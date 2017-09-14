using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Actuator/PhysicsMover")]
    public class PhysicsMover : GameScript
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private float rotationSpeed;

        private new ITransform transform;
        private new IRigidbody2D rigidbody2D;
        private ITime time;

        public void InjectPhysicsMover(float speed,
                                          float rotationSpeed,
                                          [TopParentScope] ITransform transform,
                                          [TopParentScope] IRigidbody2D rigidbody2D,
                                          [ApplicationScope] ITime time)
        {
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
            this.transform = transform;
            this.rigidbody2D = rigidbody2D;
            this.time = time;
        }

        public void Awake()
        {
            InjectDependencies("InjectPhysicsMover", speed, rotationSpeed);
        }

        public virtual void AddFowardImpulse(float force = 1)
        {
            AddImpulse(transform.Up, force);
        }

        public virtual void AddBackwardImpulse(float force = 1)
        {
            AddImpulse(-transform.Up, force);
        }

        public virtual void AddRotateLeftImpulse(float force = 1)
        {
            AddRotationImpulse(1, force);
        }

        public virtual void AddRotateRightImpulse(float force = 1)
        {
            AddRotationImpulse(-1, force);
        }

        public virtual void AddImpulse(Vector3 direction, float force = 1)
        {
            rigidbody2D.AddForce(direction.normalized * speed * force * time.DeltaTime);
        }

        public virtual void AddImpulseToward(Vector3 destination, float force = 1)
        {
            AddImpulse(destination - transform.Position, force);
        }

        private void AddRotationImpulse(float direction, float force = 1)
        {
            rigidbody2D.AddTorque(direction * rotationSpeed * force * time.DeltaTime);
        }
    }
}