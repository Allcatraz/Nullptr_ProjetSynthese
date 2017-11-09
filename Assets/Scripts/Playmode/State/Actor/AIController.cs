using UnityEngine;

namespace ProjetSynthese
{
    public class AIController
    {
        private const float FleeRange = 2.0f;

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

        private float currentSpeedLevel;

        private const float RandomRadiusMoveRange = 5.0f;

        private const float ErrorPositionTolerance = 0.001f;

        private const float FloorYOffset = 1.0f;

        public enum ControllerMode { None, Explore, Loot, Combat, Flee, Hunt }
        private ControllerMode aiControllerMode;
        private readonly ActorAI Actor;

        public AIController(ActorAI actor)
        {
            this.Actor = actor;
            MapDestinationIsKnown = false;
            OpponentTargetDestinationIsKnown = false;
            ItemTargetDestinationIsKnown = false;
            SetAIControllerMode(ControllerMode.None);
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

                        Actor.EquipmentManager.GetWeapon().Use();
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
            float pas = this.currentSpeedLevel * Time.deltaTime;
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
            Vector3 nouvellePosition = Vector3.MoveTowards(actor.transform.position, destination, pas);
            Vector3 mouvement = new Vector3(nouvellePosition.x - actor.transform.position.x, nouvellePosition.y - actor.transform.position.y, nouvellePosition.z - actor.transform.position.z);

            float angle = Mathf.Atan2(mouvement.x, mouvement.z) * Mathf.Rad2Deg;
            actor.transform.position = nouvellePosition;
            actor.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        }

        public void GenerateRandomDestination(ActorAI actor)
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
            MapDestinationIsKnown = true;
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

            if (actor.Brain.AiInPerceptionRange != null)
            {
                fleeDirection = -(actor.Brain.AiInPerceptionRange.transform.position - aiCurrentPosition);
                fleeDirection.Normalize();
                fleeDirection *= FleeRange;
                fleeDestination.x += fleeDirection.x;
                fleeDestination.z += fleeDirection.z;
            }
            else if (actor.Brain.PlayerInPerceptionRange != null)
            {
                fleeDirection = -(actor.Brain.PlayerInPerceptionRange.transform.position - aiCurrentPosition);
                fleeDirection.Normalize();
                fleeDirection *= FleeRange;
                fleeDestination.x += fleeDirection.x;
                fleeDestination.z += fleeDirection.z;
            }
            else
            {

                Vector3 unitZ = actor.transform.forward;
                Quaternion rotation = actor.transform.rotation;
                Quaternion rot180 = Quaternion.AngleAxis(180, actor.transform.forward);
                fleeDirection = rot180 * (rotation * unitZ);
                fleeDirection.Normalize();
                fleeDirection *= FleeRange;
                fleeDestination.x += fleeDirection.x;
                fleeDestination.z += fleeDirection.z;

            }
            if (ValidateMapDestination(fleeDestination))
            {
                MapDestination = fleeDestination;
                return true;
            }
            return false;

            //code si coincé pas de possibilité de fuite return false
            //exemple mur, circle of death, out of map

        }

        private bool ValidateMapDestination(Vector3 mapDestination)
        {
            return true;
        }

        public ControllerMode GetAIControllerMode()
        {
            return aiControllerMode;
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
                default:
                    break;
            }
        }
    }
}