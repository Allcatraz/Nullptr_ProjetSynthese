using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class PhysicsMoverTest : UnitTestCase
    {
        private const float Force = 2;
        private const float Speed = 10;
        private const float RotationSpeed = 15;
        private static readonly Vector3 UpDirection = new Vector3(20, 25, 30).normalized;
        private const float DeltaTime = 2;

        private ITransform transform;
        private IRigidbody2D rigidbody2D;
        private ITime time;
        private PhysicsMover physicsMover;

        [SetUp]
        public void Before()
        {
            transform = CreateSubstitute<ITransform>();
            rigidbody2D = CreateSubstitute<IRigidbody2D>();
            time = CreateSubstitute<ITime>();
            physicsMover = CreateBehaviour<PhysicsMover>();

            transform.Up.Returns(UpDirection);
            time.DeltaTime.Returns(DeltaTime);
        }

        [Test]
        public void CanMoveFoward()
        {
            Initialize();

            physicsMover.AddFowardImpulse(Force);

            CheckRigidBodyReceivedForceOf(UpDirection.x * Speed * Force * DeltaTime,
                                          UpDirection.y * Speed * Force * DeltaTime);
        }

        [Test]
        public void CanMoveBackward()
        {
            Initialize();

            physicsMover.AddBackwardImpulse(Force);

            CheckRigidBodyReceivedForceOf(-UpDirection.x * Speed * Force * DeltaTime,
                                          -UpDirection.y * Speed * Force * DeltaTime);
        }

        [Test]
        public void CanRotateLeft()
        {
            Initialize();

            physicsMover.AddRotateLeftImpulse(Force);

            CheckRigidBodyReceivedTorqueOf(RotationSpeed * Force * DeltaTime);
        }

        [Test]
        public void CanRotateRight()
        {
            Initialize();

            physicsMover.AddRotateRightImpulse(Force);

            CheckRigidBodyReceivedTorqueOf(-RotationSpeed * Force * DeltaTime);
        }

        [Test]
        public void CanMoveTowardDirection()
        {
            Vector3 direction = new Vector3(5, -5, 0);
            Vector3 normalizedDirection = direction.normalized;
            Initialize();

            physicsMover.AddImpulse(direction, Force);

            CheckRigidBodyReceivedForceOf(normalizedDirection.x * Speed * Force * DeltaTime,
                                          normalizedDirection.y * Speed * Force * DeltaTime);
        }

        [Test]
        public void CanMoveTowardPoint()
        {
            Vector3 position = new Vector3(10, 15, 0);
            Vector3 destination = new Vector3(5, -5, 0);
            Vector3 normalizedDirection = (destination - position).normalized;
            Initialize();
            SetPosition(10, 15);

            physicsMover.AddImpulseToward(destination, Force);

            CheckRigidBodyReceivedForceOf(normalizedDirection.x * Speed * Force * DeltaTime,
                                          normalizedDirection.y * Speed * Force * DeltaTime);
        }

        private void Initialize()
        {
            physicsMover.InjectPhysicsMover(Speed,
                                                  RotationSpeed,
                                                  transform,
                                                  rigidbody2D,
                                                  time);
            physicsMover.Awake();
        }

        private void SetPosition(float x, float y)
        {
            transform.Position.Returns(new Vector3(x, y, 0));
        }

        private void CheckRigidBodyReceivedForceOf(float x, float y)
        {
            rigidbody2D.Received().AddForce(Arg.Is<Vector2>(vector => vector.x == x && vector.y == y));
        }

        private void CheckRigidBodyReceivedTorqueOf(float rotation)
        {
            rigidbody2D.Received().AddTorque(rotation);
        }
    }
}