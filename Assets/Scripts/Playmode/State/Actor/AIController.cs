﻿using UnityEngine;
using UnityEngine.AI;
namespace ProjetSynthese
{
    public class AIController
    {
        private const float FleeRange = 2.0f;
        private const int PathVectorIndex = 1;

        public bool IsInBuilding { get; set; }

        public Vector3 MapDestination { get; set; }
        public Vector3 OpponentTargetDestination { get; set; }
        public Vector3 ItemTargetDestination { get; set; }

        public bool MapDestinationIsKnown { get; set; }
        public bool OpponentTargetDestinationIsKnown { get; set; }
        public bool ItemTargetDestinationIsKnown { get; set; }

        public enum MoveTarget { Item, Map, Opponent };
        public MoveTarget AIMoveTarget { get; set; }

        public enum SpeedLevel { None, Walking, Jogging, Running, Swimming };

        private SpeedLevel aiSpeed;
        public SpeedLevel AISpeed
        {
            get
            {
                return aiSpeed;
            }
            set
            {
                aiSpeed = value;
                switch (aiSpeed)
                {
                    case SpeedLevel.None:
                        currentSpeedLevel = NoSpeed;
                        break;
                    case SpeedLevel.Walking:
                        currentSpeedLevel = WalkingSpeed;
                        break;
                    case SpeedLevel.Jogging:
                        currentSpeedLevel = JoggingSpeed;
                        break;
                    case SpeedLevel.Running:
                        currentSpeedLevel = RunningSpeed;
                        break;
                    case SpeedLevel.Swimming:
                        currentSpeedLevel = SwimmingSpeed;
                        break;
                    default:
                        break;
                }
            }
        }

        private const float WalkingSpeed = 4.0f;
        private const float JoggingSpeed = 5.5f;
        private const float RunningSpeed = 7.0f;
        private const float SwimmingSpeed = 0.5f;
        private const float NoSpeed = 0.0f;
        private readonly float RandomSpeedFactor = 1.0f;
        private const float RandomSpeedMinRange = 0.9f;
        private const float RandomSpeedMaxRange = 1.1f;

        private float currentSpeedLevel;

        private const float RandomRadiusMoveRange = 5.0f;
        private const int MaxValidRandomDestinationTryPerUpdate = 3;

        private const float ErrorPositionTolerance = 1.5f;

        public const float FloorYOffset = 0.5f;
        private const float SwimYOffset = -1.0f;

        public enum ControllerMode { None, Explore, Loot, Combat, Flee, Hunt, DeathCircle }
        private ControllerMode aiControllerMode;
        private readonly ActorAI Actor;

        public AIController(ActorAI actor)
        {
            this.Actor = actor;
            MapDestinationIsKnown = false;
            OpponentTargetDestinationIsKnown = false;
            ItemTargetDestinationIsKnown = false;
            SetAIControllerMode(ControllerMode.None);
            RandomSpeedFactor = Random.Range(RandomSpeedMinRange, RandomSpeedMaxRange);
        }

        public bool HasReachedMapDestination(ActorAI actor)
        {

            float distance = Vector3.Distance(MapDestination, actor.transform.position);
            if (distance < ErrorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public bool HasReachedOpponentTargetDestination(ActorAI actor)
        {
            if (actor.Brain.IsOpponentInWeaponRange())
            {
                return true;
            }
            return false;
        }

        public bool HasReachedItemTargetDestination(ActorAI actor)
        {
            float distance = Vector3.Distance(ItemTargetDestination, actor.transform.position);
            if (distance < ErrorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public void Shoot(AIBrain.OpponentType opponentType)
        {
            switch (opponentType)
            {
                case AIBrain.OpponentType.None:
                    break;
                case AIBrain.OpponentType.AI:

                case AIBrain.OpponentType.Player:
                    if (Actor.EquipmentManager.WeaponReadyToUse())
                    {
                        Weapon weapon = Actor.EquipmentManager.GetWeapon();
                        weapon.Use();
                        Actor.CreateBullet(weapon.SpawnPoint.transform.position, weapon.SpawnPoint.transform.rotation, weapon.Chamber.transform.position, weapon.BulletSpeed, weapon.LivingTime, weapon.Dommage);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Move(ActorAI actor)
        {
            MoveDestination(AIMoveTarget, actor);
        }

        private void MoveDestination(MoveTarget moveTarget, ActorAI actor)
        {
            float pas = this.currentSpeedLevel * RandomSpeedFactor * Time.deltaTime;
            Vector3 destination = Vector3.zero;

            switch (moveTarget)
            {
                case MoveTarget.Item:
                    destination = ItemTargetDestination;
                    break;
                case MoveTarget.Map:
                    destination = MapDestination;
                    break;
                case MoveTarget.Opponent:
                    destination = OpponentTargetDestination;
                    break;
                default:
                    break;
            }

            destination.y = FloorYOffset;

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(actor.transform.position, destination, NavMesh.AllAreas, path);
            Vector3[] pathpoints;
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                pathpoints = new Vector3[path.corners.Length];
                int nbr = path.GetCornersNonAlloc(pathpoints);
                if (nbr > PathVectorIndex)
                {
                    destination = pathpoints[PathVectorIndex];
                }
            }

            destination.y = FloorYOffset;

            Vector3 nouvellePosition = Vector3.MoveTowards(actor.transform.position, destination, pas);
            Vector3 mouvement = new Vector3(nouvellePosition.x - actor.transform.position.x, nouvellePosition.y - actor.transform.position.y, nouvellePosition.z - actor.transform.position.z);

            float angle = Mathf.Atan2(mouvement.x, mouvement.z) * Mathf.Rad2Deg;
            actor.transform.position = nouvellePosition;
            actor.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        }

        public void GenerateRandomDestination(ActorAI actor)
        {

            for (int i = 0; i < MaxValidRandomDestinationTryPerUpdate; i++)
            {
                Vector3 newDestination = actor.transform.position;
                float xOffset = Random.Range(0.0f, RandomRadiusMoveRange);
                float zOffset = RandomRadiusMoveRange - xOffset;
                float signXOffset = (Random.Range(0, 2) * 2) - 1;
                float signYOffset = (Random.Range(0, 2) * 2) - 1;
                //
                newDestination.x += signXOffset * xOffset;
                newDestination.z += signYOffset * zOffset;
                newDestination.y = FloorYOffset;
                MapDestination = newDestination;
                if (IsDestinationOutOfMap(newDestination))
                {
                    newDestination.x = -newDestination.x;
                    newDestination.z = -newDestination.z;
                    MapDestination = newDestination;
                }
                
                if (!actor.Brain.DestinationOutsideDeathCircle(MapDestination))
                {
                    MapDestinationIsKnown = true;
                    break;
                }
                else
                {
                    MapDestinationIsKnown = false;
                }
            }
        }

        public void FindTargetItemMapDestination(ActorAI actor)
        {
            Item item = actor.Sensor.NeareastNonEquippedItem(actor.transform.position);
            actor.Brain.UpdateItemOnMapKnowledge(item);
            if (item != null)
            {
                Vector3 newDestination = Vector3.zero;
                newDestination.x = item.transform.position.x;
                newDestination.z = item.transform.position.z;
                newDestination.y = FloorYOffset;
                ItemTargetDestination = newDestination;

                ItemTargetDestinationIsKnown = true;
            }
            else
            {
                ItemTargetDestinationIsKnown = false;
            }

        }

        public void FindTargetOpponnentMapDestination(ActorAI actor)
        {

            PlayerController opponentPlayer = null;
            ActorAI opponentAI = null;
            opponentPlayer = actor.Sensor.NeareastGameObject<PlayerController>(actor.transform.position, AIRadar.LayerType.Player);
            opponentAI = actor.Sensor.NeareastNonAllyAI(actor);
            actor.Brain.UpdateOpponentOnMapKnowledge(opponentPlayer, opponentAI);
            Vector3 newDestination = Vector3.zero;
            newDestination.y = FloorYOffset;
            if (opponentPlayer != null)
            {

                newDestination.x = opponentPlayer.transform.position.x;
                newDestination.z = opponentPlayer.transform.position.z;
                OpponentTargetDestination = newDestination;
                OpponentTargetDestinationIsKnown = true;
                actor.Brain.CurrentOpponentType = AIBrain.OpponentType.Player;
            }
            else if (opponentAI != null)
            {
                newDestination.x = opponentAI.transform.position.x;
                newDestination.z = opponentAI.transform.position.z;
                OpponentTargetDestination = newDestination;
                OpponentTargetDestinationIsKnown = true;
                actor.Brain.CurrentOpponentType = AIBrain.OpponentType.AI;
            }
            else
            {
                OpponentTargetDestinationIsKnown = false;
            }
        }

        public void SetDeathCircleFleeDestination(ActorAI actor)
        {

            if (actor.Brain.ExistVisibleOpponent() && FoundFleeDestination(actor))
            {
                //Safe circle flee direction
                Vector3 fleeDirection = Vector3.zero;
                Vector3 aiCurrentPosition = actor.transform.position;
                Vector3 fleeDestination = aiCurrentPosition;
                fleeDirection = actor.Brain.SafeCircleCenterPosition - aiCurrentPosition;
                fleeDirection.y = FloorYOffset;
                fleeDirection.Normalize();

                Vector3 opponentEscapingDirection = MapDestination;
                opponentEscapingDirection.y = FloorYOffset;
                opponentEscapingDirection.Normalize();
                float scalarProduct = Vector3.Dot(fleeDirection, opponentEscapingDirection);

                float opponentPositionFactor = 1.0f;
                if (actor.Brain.CurrentDistanceOutsideSafeCircle > 0.0f)
                {
                    float expectedLifePointLoss = (actor.Brain.CurrentDistanceOutsideSafeCircle / currentSpeedLevel) * actor.Brain.CurrentDeathCircleHurtPoints;
                    opponentPositionFactor = (actor.AIHealth.HealthPoints - expectedLifePointLoss) / actor.AIHealth.HealthPoints;
                    if (opponentPositionFactor < 0.0f)
                    {
                        opponentPositionFactor = 0.0f;
                    }
                }

                if (scalarProduct > 0.0f)
                {
                    fleeDirection = opponentPositionFactor * opponentEscapingDirection + fleeDirection;
                }
                else
                {
                    opponentEscapingDirection = -opponentEscapingDirection;
                    Vector3 yPerpendicularVector = Vector3.Cross(fleeDirection, opponentEscapingDirection);
                    yPerpendicularVector.Normalize();
                    opponentEscapingDirection = Vector3.Cross(fleeDirection, yPerpendicularVector);
                    opponentEscapingDirection.y = FloorYOffset;
                    opponentEscapingDirection.Normalize();
                    fleeDirection = opponentPositionFactor * opponentEscapingDirection + fleeDirection;
                    fleeDirection.y = FloorYOffset;
                }
                fleeDirection.Normalize();
                fleeDirection *= FleeRange;
                fleeDestination.x += fleeDirection.x;
                fleeDestination.z += fleeDirection.z;
                MapDestination = fleeDestination;

            }
            else
            {
                Vector3 fleeDirection = Vector3.zero;
                Vector3 aiCurrentPosition = actor.transform.position;
                Vector3 fleeDestination = aiCurrentPosition;

                fleeDirection = actor.Brain.SafeCircleCenterPosition - aiCurrentPosition;
                fleeDirection.y = FloorYOffset;
                fleeDirection.Normalize();
                fleeDirection *= FleeRange;
                fleeDestination.x += fleeDirection.x;
                fleeDestination.z += fleeDirection.z;
                MapDestination = fleeDestination;
            }
            MapDestinationIsKnown = true;
        }

        public void SetFleeDestination(ActorAI actor)
        {

            if (FoundFleeDestination(actor))
            {
                MapDestinationIsKnown = true;
            }
            else
            {
                MapDestinationIsKnown = false;
            }

        }

        private bool FoundFleeDestination(ActorAI actor)
        {
            Vector3 fleeDirection = Vector3.zero;
            Vector3 aiCurrentPosition = actor.transform.position;
            Vector3 fleeDestination = aiCurrentPosition;

            if (actor.Brain.PlayerInPerceptionRange != null && aiCurrentPosition != null)
            {
                fleeDirection = -(actor.Brain.PlayerInPerceptionRange.transform.position - aiCurrentPosition);
                fleeDirection.Normalize();
                fleeDirection *= FleeRange;
                fleeDestination.x += fleeDirection.x;
                fleeDestination.z += fleeDirection.z;
            }
            else if (actor.Brain.AiInPerceptionRange != null && aiCurrentPosition != null)
            {
                fleeDirection = -(actor.Brain.AiInPerceptionRange.transform.position - aiCurrentPosition);
                fleeDirection.Normalize();
                fleeDirection *= FleeRange;
                fleeDestination.x += fleeDirection.x;
                fleeDestination.z += fleeDirection.z;
            }
            else
            {
                return false;
            }

            fleeDestination.y = FloorYOffset;

            if (ValidateMapDestination(fleeDestination))
            {
                MapDestination = fleeDestination;
                return true;
            }
            else
            {
                Vector3 actualInvalideFleeDirection = fleeDestination - aiCurrentPosition;
                Vector3 up = new Vector3(0, 1, 0);
                Vector3 down = new Vector3(0, -1, 0);
                Vector3 upPerpendicularFleeDirection = Vector3.Cross(actualInvalideFleeDirection, up);
                Vector3 downPerpendicularFleeDirection = Vector3.Cross(actualInvalideFleeDirection, down);
                upPerpendicularFleeDirection.Normalize();
                upPerpendicularFleeDirection *= FleeRange;
                downPerpendicularFleeDirection.Normalize();
                downPerpendicularFleeDirection *= FleeRange;

                Vector3 fleeDestinationUp = aiCurrentPosition;
                fleeDestinationUp.x += upPerpendicularFleeDirection.x;
                fleeDestinationUp.z += upPerpendicularFleeDirection.z;
                fleeDestinationUp.y = FloorYOffset;

                Vector3 fleeDestinationDown = aiCurrentPosition;
                fleeDestinationDown.x += downPerpendicularFleeDirection.x;
                fleeDestinationDown.z += downPerpendicularFleeDirection.z;
                fleeDestinationDown.y = FloorYOffset;
                if (ValidateMapDestination(fleeDestinationDown))
                {
                    MapDestination = fleeDestinationDown;
                    return true;
                }
                else if (ValidateMapDestination(fleeDestinationUp))
                {
                    MapDestination = fleeDestinationUp;
                    return true;
                }
            }
            return false;
        }

        private bool ValidateMapDestination(Vector3 mapDestination)
        {
            if (IsDestinationOutOfMap(mapDestination))
            {
               return false;
            }
            if (Actor.Brain.DestinationOutsideDeathCircle(mapDestination))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ControllerMode GetAIControllerMode()
        {
            return aiControllerMode;
        }

        public void SwitchOnSwimPosition()
        {
            Actor.transform.position = new Vector3(Actor.transform.position.x, SwimYOffset, Actor.transform.position.z);
            Actor.GetComponent<Rigidbody>().useGravity = false;
        }
        public void SwitchOffSwimPosition()
        {
            Actor.transform.position = new Vector3(Actor.transform.position.x, FloorYOffset, Actor.transform.position.z);
            Actor.GetComponent<Rigidbody>().useGravity = true;
        }

        public void SetAIControllerMode(ControllerMode controllerMode)
        {
            aiControllerMode = controllerMode;
            switch (controllerMode)
            {
                case ControllerMode.None:
                    AISpeed = AIController.SpeedLevel.None;
                    Actor.Sensor.AIPerceptionLevel = AIRadar.PerceptionLevel.None;
                    break;
                case ControllerMode.Explore:
                    AISpeed = AIController.SpeedLevel.Walking;
                    Actor.Sensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
                    break;
                case ControllerMode.Loot:
                    AISpeed = AIController.SpeedLevel.Jogging;
                    Actor.Sensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
                    break;
                case ControllerMode.Combat:
                    AISpeed = AIController.SpeedLevel.Jogging;
                    Actor.Sensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
                    break;
                case ControllerMode.Flee:
                    AISpeed = AIController.SpeedLevel.Running;
                    Actor.Sensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
                    break;
                case ControllerMode.Hunt:
                    AISpeed = AIController.SpeedLevel.Running;
                    Actor.Sensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
                    break;
                case ControllerMode.DeathCircle:
                    AISpeed = AIController.SpeedLevel.Running;
                    Actor.Sensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
                    break;
                default:
                    break;
            }
        }

        private bool IsDestinationOutOfMap(Vector3 destination)
        {
            bool outOfMap = false;
            if (destination.x < AISpawnerController.XMapOriginCornerCoordinate
                || destination.x > AISpawnerController.XMapOriginOppositeCornerCoordinate
                || destination.z > AISpawnerController.ZMapOriginCornerCoordinate
                || destination.z < AISpawnerController.ZMapOriginOppositeCornerCoordinate)
            {
                outOfMap = true;
            }

            return outOfMap;
        }
    }
}