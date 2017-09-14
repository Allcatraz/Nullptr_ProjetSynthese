using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DeathEventPublisherTest : UnitTestCase
    {
        private const R.E.Prefab Prefab = R.E.Prefab.Player;

        private Health health;
        private DeathEventChannel eventChannel;
        private DeathEventPublisher eventPublisher;

        [SetUp]
        public void Before()
        {
            health = CreateSubstitute<Health>();
            eventChannel = CreateSubstitute<DeathEventChannel>();
            eventPublisher = CreateBehaviour<DeathEventPublisher>();
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
        public void OnDeathPublishEvent()
        {
            Initialize();

            Die();

            CheckEventPublished();
        }

        private void Initialize()
        {
            eventPublisher.InjectDeathEventPublisher(Prefab, health, eventChannel);
            eventPublisher.Awake();
            eventPublisher.OnEnable();
        }

        private void Disable()
        {
            eventPublisher.OnDisable();
        }

        private void Die()
        {
            health.OnDeath += Raise.Event<DeathEventHandler>();
        }

        private void CheckEventPublished()
        {
            eventChannel.Received().Publish(Arg.Is<DeathEvent>(deathEvent =>
                                                                   deathEvent.DeadPrefab == Prefab));
        }
    }
}