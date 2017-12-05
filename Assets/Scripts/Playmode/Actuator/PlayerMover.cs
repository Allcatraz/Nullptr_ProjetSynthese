using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    public delegate void MoveEventHandler();
    public delegate void SpeedChangeEvent(float newSpeed);

    [AddComponentMenu("Game/Actuator/PlayerMover")]
    public class PlayerMover : GameScript
    {
        [Tooltip("La vitesse du joueur dans son déplacement normal.")]
        [SerializeField] private float moveSpeed;
        [Tooltip("La vitesse du joueur dans son déplacement de sprint.")]
        [SerializeField] private float sprintSpeed;
        [Tooltip("La vitesse du joueur lors de son déplcament à la nage normal.")]
        [SerializeField] private float swimSpeed;

        private Rigidbody rigidbody;
        private DeathCircleDistanceEventChannel deathCircleDistanceEvent;

        private float speed = 0;
        public float Speed
        {
            get
            {
                return speed;
            }            
            private set
            {
                speed = value;
                if (OnSpeedChange != null)
                {
                    OnSpeedChange(speed);
                }
            }
        }

        public event MoveEventHandler OnMove;
        public event SpeedChangeEvent OnSpeedChange;

        private void InjectPlayerMover([RootScope] Rigidbody rigidbody,
                                       [EventChannelScope] DeathCircleDistanceEventChannel deathCircleDistanceEvent)
        {
            this.rigidbody = rigidbody;
            this.deathCircleDistanceEvent = deathCircleDistanceEvent;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerMover");
            deathCircleDistanceEvent.OnEventPublished += UpdateMove;
            rigidbody.rotation = Quaternion.identity;
            Speed = moveSpeed;
        }

        public void SwitchSprintOn()
        {
            Speed = sprintSpeed;
        }

        public void SwitchSprintOff()
        {
            Speed = moveSpeed;
        }

        public void SwitchSwimOn()
        {
            Speed = swimSpeed;
        }

        public void SwitchSwimOff()
        {
            Speed = moveSpeed;
        }

        public void Move(Vector3 direction)
        {
            rigidbody.velocity = direction * speed;
            UpdateMove(new DeathCircleDistanceEvent(0, 0, Vector3.zero));
        }

        public void UpdateMove(DeathCircleDistanceEvent deathCircleDistanceEvent)
        {
            if (OnMove != null) OnMove();
        }

        public void Rotate(Vector3 angle)
        {
            if (Input.GetKey(KeyCode.LeftControl) != true)
            {
                rigidbody.transform.eulerAngles = angle;            
            }
        }

        public float GetNormalMoveSpeed()
        {
            return moveSpeed;
        }
    }
}