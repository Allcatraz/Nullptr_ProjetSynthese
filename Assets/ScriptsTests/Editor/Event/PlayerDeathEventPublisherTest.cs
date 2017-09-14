using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class PlayerDeathEventPublisherTest : UnitTestCase
    {
        private Health health;
        private PlayerDeathEventChannel eventChannel;
        private PlayerDeathEventPublisher eventPublisher;

        [SetUp]
        public void Before()
        {
            health = CreateSubstitute<Health>();
            eventChannel = CreateSubstitute<PlayerDeathEventChannel>();
            eventPublisher = CreateBehaviour<PlayerDeathEventPublisher>();
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
            eventPublisher.InjectPlayerDeathEventPublisher(health, eventChannel);
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
            eventChannel.Received().Publish(Arg.Any<PlayerDeathEvent>());
        }
    }
}