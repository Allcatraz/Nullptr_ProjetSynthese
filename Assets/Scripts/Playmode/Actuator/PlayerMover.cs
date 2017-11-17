using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    public delegate void MoveEventHandler();

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
        public float Speed { get; private set; }

        public event MoveEventHandler OnMove;

        private void InjectPlayerMover([RootScope] Rigidbody rigidbody)
        {
            this.rigidbody = rigidbody;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerMover");
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
            rigidbody.velocity = direction * Speed;
            if (OnMove != null) OnMove();
        }

        public void Rotate(Vector3 angle)
        {
            if (Input.GetKey(KeyCode.LeftControl) != true)
            {
                rigidbody.transform.eulerAngles = angle;
                
            }
        }
    }
}