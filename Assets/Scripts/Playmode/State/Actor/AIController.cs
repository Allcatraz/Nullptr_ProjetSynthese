using UnityEngine;

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
        [SerializeField]
        private const float WalkingSpeed = 7.5f;
        [SerializeField]
        private const float JoggingSpeed = 10.0f;
        [SerializeField]
        private const float RunningSpeed = 5.0f;
        [SerializeField]
        private const float SwimmingSpeed = 0.5f;

        private const float NoSpeed = 0.0f;

        private float currentSpeedLevel;

        private const float RandomRadiusMoveRange = 200.0f;

        private const float ErrorPositionTolerance = 0.001f;

        private const float FloorYOffset = 10.0f;

        public enum ControllerMode { None, Explore, Loot,Combat }
        private ControllerMode aiControllerMode;
        private readonly ActorAI Actor;

        public AIController(ActorAI actor)
        {
            this.Actor = actor;
        }

        public void Init()
        {
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

            float distance = Vector3.Distance(OpponentTargetDestination, actor.transform.position);
            if (distance < ErrorPositionTolerance)
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

        public override void Move(ActorAI actor)
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
                newDestination.y = 10.0f;
                ItemTargetDestination = newDestination;

                ItemTargetDestinationIsKnown = true;
            }
            else
            {
                ItemTargetDestinationIsKnown = false;
            }
           
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
                default:
                    break;
            }
        }
    }
}