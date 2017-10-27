using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    public delegate void MoveEventHandler();
    
    //BEN_REVIEW : When your code is just the right size...
    //             http://i0.kym-cdn.com/photos/images/facebook/001/070/061/d96.jpg
    
    [AddComponentMenu("Game/Actuator/PlayerMover")]
    public class PlayerMover : GameScript
    {
        [Tooltip("La vitesse du joueur dans son déplacement normal.")]
        [SerializeField] private float moveSpeed;
        [Tooltip("La vitesse du joueur dans son déplacement de sprint.")]
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

        //BEN_REVIEW : Sugestion. Puisque la rotation est "instantanée", comparativement à "Move"
        //             pourquoi pas appeler ça "RotateImmediate" ?
        public void Rotate(Vector3 angle)
        {
            if (Input.GetKey(KeyCode.LeftControl) != true)
            {
                topParentTransform.eulerAngles = angle;
            }
        }
    }
}