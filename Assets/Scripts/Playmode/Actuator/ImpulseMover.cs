using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/ImpulseMover")]
    public class ImpulseMover : GameScript
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private float rotationSpeed;

        private Transform topParentTransform;
        private Rigidbody2D topParentRigidbody2D;

        private void InjectImpulseMover([TopParentScope] Transform topParentTransform,
                                       [TopParentScope] Rigidbody2D topParentRigidbody2D)
        {
            this.topParentTransform = topParentTransform;
            this.topParentRigidbody2D = topParentRigidbody2D;
        }

        private void Awake()
        {
            InjectDependencies("InjectImpulseMover");
        }

        public void AddFowardImpulse(float force = 1)
        {
            AddImpulse(topParentTransform.up, force);
        }

        public void AddBackwardImpulse(float force = 1)
        {
            AddImpulse(-topParentTransform.up, force);
        }

        public void AddRotateLeftImpulse(float force = 1)
        {
            AddRotationImpulse(1, force);
        }

        public void AddRotateRightImpulse(float force = 1)
        {
            AddRotationImpulse(-1, force);
        }

        public void AddImpulse(Vector3 direction, float force = 1)
        {
            topParentRigidbody2D.AddForce(direction.normalized * speed * force * Time.deltaTime);
        }

        public void AddImpulseToward(Vector3 destination, float force = 1)
        {
            AddImpulse(destination - topParentTransform.position, force);
        }

        public void AddRotationImpulse(float direction, float force = 1)
        {
            topParentRigidbody2D.AddTorque(direction * rotationSpeed * force * Time.deltaTime);
        }
    }
}