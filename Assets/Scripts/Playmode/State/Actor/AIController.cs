﻿using UnityEngine;

namespace ProjetSynthese
{
    public class AIController : ActorController
    {
        public bool IsInBuilding { get; set; }

        public Vector3 MapDestination { get; set; }
        public Vector3 OpponentTargetDestination { get; set; }
        public Vector3 ItemTargetDestination { get; set; }

        public bool MapDestinationIsKnown { get; set; }
        public bool OpponentTargetDestinationIsKnown { get; set; }
        public bool ItemTargetDestinationIsKnown { get; set; }

        public enum MoveTarget { Item, Map, Opponent };
        public MoveTarget AIMoveTarget { get; set; }

        public enum SpeedLevel { Walking, Jogging, Running, Swimming };

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
        [SerializeField]
        private const float WalkingSpeed = 0.75f;
        [SerializeField]
        private const float JoggingSpeed = 2.0f;
        [SerializeField]
        private const float RunningSpeed = 5.0f;
        [SerializeField]
        private const float SwimmingSpeed = 0.5f;

        private float currentSpeedLevel;

        private const float RandomRadiusMoveRange = 5.0f;

        private const float errorPositionTolerance = 0.001f;

        public enum ControllerMode { None,Explore }
        public ControllerMode AIControllerMode {get;private set;}

        public AIRadar AISensor { get; private set; }

        private void Start()
        {
            MapDestinationIsKnown = false;
            OpponentTargetDestinationIsKnown = false;
            ItemTargetDestinationIsKnown = false;
            AISensor = new AIRadar();
            AIControllerMode = ControllerMode.None;
        }

        public bool HasReachedMapDestination(ActorAI actor)
        {

            float distance = Vector3.Distance(MapDestination, actor.transform.position);
            if (distance < errorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public bool HasReachedOpponentTargetDestination(ActorAI actor)
        {

            float distance = Vector3.Distance(OpponentTargetDestination, actor.transform.position);
            if (distance < errorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public bool HasReachedItemTargetDestination(ActorAI actor)
        {

            float distance = Vector3.Distance(ItemTargetDestination, actor.transform.position);
            if (distance < errorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public override void Move(ActorAI actor)
        {
            MoveDestination(AIMoveTarget, actor);
        }

        private void MoveDestination(MoveTarget moveTarget,ActorAI actor)
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

            ////La méthode MoveTowards de vector2 fait pas mal la job de déplacement pour nous.  Pour le moment elle va chercher la nouvelle position, mais le déplacement n'est pas encore fait.
            Vector3 nouvellePosition = Vector3.MoveTowards(actor.transform.position, destination, pas);

            ////Différence entre la nouvelle et l'ancienne position, pour calculer l'angle de rotation
            Vector3 mouvement = new Vector3(nouvellePosition.x - actor.transform.position.x, nouvellePosition.y - actor.transform.position.y, nouvellePosition.z - actor.transform.position.z);

            ////Angle de rotation en degrée (car l'affichage de Unity veut les angles en degrée); Mathf.Rad2Deg nécessaire car Mathf.Atan2 un résultat en radiants
           float angle = Mathf.Atan2(mouvement.x, mouvement.z) * Mathf.Rad2Deg;

            ////On applique la nouvelle position
            actor.transform.position = nouvellePosition;

            ////On applique la nouvelle rotation
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
            MapDestination = newDestination;
            MapDestinationIsKnown = true;
        }

        public void SetAIControllerMode(ControllerMode controllerMode)
        {
            AIControllerMode = controllerMode;
            switch (controllerMode)
            {
                case ControllerMode.Explore:
                    AISpeed = AIController.SpeedLevel.Walking;
                    AISensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
                    break;
                default:
                    break;
            }

            
        }
    }
}