using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/PlayerMover")]
    public class PlayerMover : NetworkGameScript
    {
        [SerializeField] private float speed;

        private Transform topParentTransform;

        private void InjectImpulseMover([TopParentScope] Transform topParentTransform)
        {
            this.topParentTransform = topParentTransform;
        }

        private void Awake()
        {
            InjectDependencies("InjectImpulseMover");
        }

        public void Move(Vector3 direction)
        {
            topParentTransform.Translate(direction * speed * Time.deltaTime);
        }

        public void Rotate()
        {
            Vector2 lookAt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angleBetween = Vector2.SignedAngle(Vector2.up, lookAt);
            transform.localEulerAngles = new Vector3(0, 0, angleBetween);
        }
    }
}