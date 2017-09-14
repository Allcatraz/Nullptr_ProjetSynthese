using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DestroyOnDeathTest : UnitTestCase
    {
        private Health health;
        private EntityDestroyer entityDestroyer;
        private DestroyOnDeath destroyOnDeath;

        [SetUp]
        public void Before()
        {
            health = CreateSubstitute<Health>();
            entityDestroyer = CreateSubstitute<EntityDestroyer>();
            destroyOnDeath = CreateBehaviour<DestroyOnDeath>();
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
        public void DestroyGameObjectOnDeath()
        {
            Initialize();

            Die();

            CheckGameObjectIsDestroyed();
        }

        private void Initialize()
        {
            destroyOnDeath.InjectDestroyOnDeath(health, entityDestroyer);
            destroyOnDeath.Awake();
            destroyOnDeath.OnEnable();
        }

        private void Disable()
        {
            destroyOnDeath.OnDisable();
        }

        public void Die()
        {
            health.OnDeath += Raise.Event<DeathEventHandler>();
        }

        private void CheckGameObjectIsDestroyed()
        {
            entityDestroyer.Received().Destroy();
        }
    }
}