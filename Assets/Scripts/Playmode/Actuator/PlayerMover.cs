using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/PlayerMover")]
    public class PlayerMover : GameScript
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;

        private Transform topParentTransform;
        private float speed = 0;

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
        }

        public void Rotate()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 distance = new Vector3(mousePos.x - topParentTransform.position.x, mousePos.y - topParentTransform.position.y, mousePos.z - topParentTransform.position.z);
            float angle = (Mathf.Atan2(distance.x, distance.z) * 180 / Mathf.PI);
            topParentTransform.eulerAngles = new Vector3(0, angle, 0);
            StaticMinimapPass.PlayerTransform = topParentTransform;
        }
    }
}