using Harmony.EventSystem;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class PlayerHealthEventPublisherTest : UnitTestCase
    {
        private const int HealthPoints = 10;
        private const int OldHealthPoints = 10;
        private const int NewHealthPoints = 15;

        private Health health;
        private PlayerHealthEventChannel eventChannel;
        private PlayerHealthEventPublisher eventPublisher;

        [SetUp]
        public void Before()
        {
            health = CreateSubstitute<Health>();
            eventChannel = CreateSubstitute<PlayerHealthEventChannel>();
            eventPublisher = CreateBehaviour<PlayerHealthEventPublisher>();

            health.HealthPoints.Returns(HealthPoints);
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            health.Received().OnHealthChanged += Arg.Any<HealthChangedEventHandler>();
            eventChannel.Received().OnUpdateRequested += Arg.Any<EventChannelUpdateRequestHandler<PlayerHealthUpdate>>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            health.Received().OnHealthChanged -= Arg.Any<HealthChangedEventHandler>();
            eventChannel.Received().OnUpdateRequested -= Arg.Any<EventChannelUpdateRequestHandler<PlayerHealthUpdate>>();
        }

        [Test]
        public void OnHealthChangedPublishEvent()
        {
            Initialize();

            ChangeHealth();

            CheckEventPublished();
        }

        [Test]
        public void OnUpdateRequestedPublishUpdate()
        {
            Initialize();

            EventChannelUpdateHandler<PlayerHealthUpdate> handler = CreateSubstitute<EventChannelUpdateHandler<PlayerHealthUpdate>>();
            RequestUpdate(handler);

            handler.Received()(Arg.Is<PlayerHealthUpdate>(healthEvent => healthEvent.PlayerHealth == health));
        }

        private void Initialize()
        {
            eventPublisher.InjectPlayerHealthEventPublisher(health, eventChannel);
            eventPublisher.Awake();
            eventPublisher.OnEnable();
        }

        private void Disable()
        {
            eventPublisher.OnDisable();
        }

        private void ChangeHealth()
        {
            health.OnHealthChanged += Raise.Event<HealthChangedEventHandler>(OldHealthPoints, NewHealthPoints);
        }

        private void RequestUpdate(EventChannelUpdateHandler<PlayerHealthUpdate> handler)
        {
            eventChannel.OnUpdateRequested += Raise.Event<EventChannelUpdateRequestHandler<PlayerHealthUpdate>>(handler);
        }

        private void CheckEventPublished()
        {
            eventChannel.Received().Publish(Arg.Is<PlayerHealthEvent>(healthEvent => healthEvent.PlayerHealth == health &&
                                                                                                 healthEvent.OldHealthPoints == OldHealthPoints &&
                                                                                                 healthEvent.NewHealthPoints == NewHealthPoints));
        }
    }
}