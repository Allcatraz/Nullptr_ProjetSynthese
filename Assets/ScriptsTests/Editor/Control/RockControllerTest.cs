using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class RockControllerTest : UnitTestCase
    {
        private const uint MinRockFragments = 3;
        private const uint MaxRockFragments = 5;

        private static readonly Vector3 Position = new Vector3(7, 4, 0);
        private const uint RandomNumberOfRockFragments = 4;

        private ITransform transform;
        private IRandom random;
        private Health health;
        private RandomRockShape randomRockShape;
        private PhysicsMover physicsMover;
        private RockSpawner rockSpawner;
        private RockController rockController;

        [SetUp]
        public void Before()
        {
            transform = CreateSubstitute<ITransform>();
            random = CreateSubstitute<IRandom>();
            health = CreateSubstitute<Health>();
            randomRockShape = CreateSubstitute<RandomRockShape>();
            physicsMover = CreateSubstitute<PhysicsMover>();
            rockSpawner = CreateSubstitute<RockSpawner>();
            rockController = CreateBehaviour<RockController>();

            transform.Position.Returns(Position);
            random.GetRandomUInt(MinRockFragments, MaxRockFragments).Returns(RandomNumberOfRockFragments);
        }

        [Test]
        public void WhenAwakeRegistersToEvents()
        {
            Initialize();

            health.OnDeath += Arg.Any<DeathEventHandler>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            health.OnDeath -= Arg.Any<DeathEventHandler>();
        }

        [Test]
        public void CanConfigureRock()
        {
            Initialize();

            rockController.Configure(2f, new Vector3(1, 2, 0), 4f);

            CheckRandomRockShapeSize(2f);
            CheckRockReceivedImpulse(new Vector3(1, 2, 0), 4f);
        }

        [Test]
        public void OnRockDeathExplodeToSmallerPieces()
        {
            Initialize();
            rockController.Configure(2f, new Vector3(1, 2, 0), 4f);

            Die();

            rockSpawner.Received().SpawnFragments(RandomNumberOfRockFragments, Position, 2f);
        }

        private void Initialize()
        {
            rockController.InjectRockController(MinRockFragments,
                                                MaxRockFragments,
                                                transform,
                                                random,
                                                health,
                                                randomRockShape,
                                                physicsMover,
                                                rockSpawner);
            rockController.Awake();
        }

        private void Destroy()
        {
            rockController.OnDestroy();
        }

        private void Die()
        {
            health.OnDeath += Raise.Event<DeathEventHandler>();
        }

        private void CheckRandomRockShapeSize(float size)
        {
            randomRockShape.Received().Radius = size;
        }

        private void CheckRockReceivedImpulse(Vector3 direction, float force)
        {
            physicsMover.Received().AddImpulse(direction, force);
        }
    }
}