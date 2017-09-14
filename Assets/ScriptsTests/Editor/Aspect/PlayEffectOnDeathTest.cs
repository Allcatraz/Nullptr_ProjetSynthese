using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class PlayEffectOnDeathTest : UnitTestCase
    {
        private static readonly Vector3 Position = new Vector3(1,2,3);

        private GameObject effectPrefab;
        private ITransform transform;
        private IPrefabFactory prefabFactory;
        private Health health;
        private PlayEffectOnDeath playEffectOnDeath;

        [SetUp]
        public void Before()
        {
            effectPrefab = CreateGameObject();
            transform = CreateSubstitute<ITransform>();
            prefabFactory = CreateSubstitute<IPrefabFactory>();
            health = CreateSubstitute<Health>();
            playEffectOnDeath = CreateBehaviour<PlayEffectOnDeath>();

            transform.Position.Returns(Position);
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            health.Received().OnDeath += Arg.Any<DeathEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            health.Received().OnDeath -= Arg.Any<DeathEventHandler>();
        }

        [Test]
        public void CanPlayEffectOnDeath()
        {
            Initialize();

            Die();

            CheckEffectCreated();
        }

        private void Initialize()
        {
            playEffectOnDeath.InjectPlayEffectOnDeath(effectPrefab,
                                                      transform,
                                                      prefabFactory,
                                                      health);
            playEffectOnDeath.Awake();
            playEffectOnDeath.OnEnable();
        }

        private void Disable()
        {
            playEffectOnDeath.OnDisable();
        }

        public void Die()
        {
            health.OnDeath += Raise.Event<DeathEventHandler>();
        }

        private void CheckEffectCreated()
        {
            prefabFactory.Instantiate(effectPrefab, Position, Arg.Any<Quaternion>());
        }
    }
}