using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    public delegate void MoveEventHandler();

    [AddComponentMenu("Game/Actuator/PlayerMover")]
    public class PlayerMover : GameScript
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;

        private Transform topParentTransform;
        private float speed = 0;

        public event MoveEventHandler OnMove;

        private void InjectPlayerMover([RootScope] Transform topParentTransform)
        {
            this.topParentTransform = topParentTransform;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerMover");
            topParentTransform.rotation = Quaternion.identity;
            speed = moveSpeed;
        }

        public void SwitchSprintOn()
        {
            speed = sprintSpeed;
        }

        public void SwitchSprintOff()
        {
            speed = moveSpeed;
        }

        public void Move(Vector3 direction)
        {
            topParentTransform.position += direction * speed * Time.deltaTime;
            if (OnMove != null) OnMove();
        }

        public void Rotate(Vector3 angle)
        {
            if (Input.GetKey(KeyCode.LeftControl) != true)
            {
                topParentTransform.eulerAngles = angle;
            }
        }
    }
}