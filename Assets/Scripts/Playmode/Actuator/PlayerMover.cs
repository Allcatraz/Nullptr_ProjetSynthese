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

        private float speed = 0;

        private void Awake()
        {
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
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        public void Rotate()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 distance = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, mousePos.z - transform.position.z);
            float angle = (Mathf.Atan2(distance.x, distance.z) * 180 / Mathf.PI);
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}