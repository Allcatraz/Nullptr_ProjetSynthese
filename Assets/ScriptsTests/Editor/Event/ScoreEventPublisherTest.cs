using Harmony.EventSystem;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class ScoreEventPublisherTest : UnitTestCase
    {
        private const uint ScorePoints = 10;
        private const uint OldScorePoints = 10;
        private const uint NewScorePoints = 15;

        private Score score;
        private ScoreEventChannel eventChannel;
        private ScoreEventPublisher eventPublisher;

        [SetUp]
        public void Before()
        {
            score = CreateSubstitute<Score>();
            eventChannel = CreateSubstitute<ScoreEventChannel>();
            eventPublisher = CreateBehaviour<ScoreEventPublisher>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            score.Received().OnScoreChanged += Arg.Any<ScoreChangedEventHandler>();
            eventChannel.Received().OnUpdateRequested += Arg.Any<EventChannelUpdateRequestHandler<ScoreUpdate>>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            score.Received().OnScoreChanged += Arg.Any<ScoreChangedEventHandler>();
            eventChannel.Received().OnUpdateRequested -= Arg.Any<EventChannelUpdateRequestHandler<ScoreUpdate>>();
        }

        private void Initialize()
        {
            eventPublisher.InjectScoreEventPublisher(score, eventChannel);
            eventPublisher.Awake();
            eventPublisher.OnEnable();
        }

        private void Disable()
        {
            eventPublisher.OnDisable();
        }

        [Test]
        public void OnScoreChangedPublishEvent()
        {
            Initialize();

            ChangeScore();

            CheckEventPublished();
        }

        [Test]
        public void OnUpdateRequestedPublishUpdate()
        {
            Initialize();

            EventChannelUpdateHandler<ScoreUpdate> handler = CreateSubstitute<EventChannelUpdateHandler<ScoreUpdate>>();
            RequestUpdate(handler);

            handler.Received()(Arg.Is<ScoreUpdate>(scoreEvent => scoreEvent.Score == score));
        }

        private void ChangeScore()
        {
            score.OnScoreChanged += Raise.Event<ScoreChangedEventHandler>(OldScorePoints, NewScorePoints);
        }

        private void RequestUpdate(EventChannelUpdateHandler<ScoreUpdate> handler)
        {
            eventChannel.OnUpdateRequested += Raise.Event<EventChannelUpdateRequestHandler<ScoreUpdate>>(handler);
        }

        private void CheckEventPublished()
        {
            eventChannel.Received().Publish(Arg.Is<ScoreEvent>(healthEvent => healthEvent.Score == score &&
                                                                              healthEvent.OldScorePoints == OldScorePoints &&
                                                                              healthEvent.NewScorePoints == NewScorePoints));
        }
    }
}