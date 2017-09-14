using Harmony.EventSystem;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class HeadUpMenuControllerTest : UnitTestCase
    {
        private const int HealthPoints = 70;
        private const int MaxHealthPoints = 100;
        private const int OldHealthPoints = 50;
        private const int NewHealthPoints = 70;
        private const uint ScorePoints = 70;
        private const uint OldScorePoints = 50;
        private const uint NewScorePoints = 70;

        private Health health;
        private Score score;
        private HealthBarView healthBarView;
        private ScoreView scoreView;
        private PlayerHealthEventChannel playerHealthEventChannel;
        private ScoreEventChannel scoreEventChannel;
        private HeadUpMenuController headUpMenuController;

        [SetUp]
        public void Before()
        {
            health = CreateSubstitute<Health>();
            score = CreateSubstitute<Score>();
            healthBarView = CreateSubstitute<HealthBarView>();
            scoreView = CreateSubstitute<ScoreView>();
            playerHealthEventChannel = CreateSubstitute<PlayerHealthEventChannel>();
            scoreEventChannel = CreateSubstitute<ScoreEventChannel>();
            headUpMenuController = CreateBehaviour<HeadUpMenuController>();

            health.MaxHealthPoints.Returns(MaxHealthPoints);
            health.HealthPoints.Returns(HealthPoints);
            score.ScorePoints.Returns(ScorePoints);
        }

        [Test]
        public void WhenCreatedRegistersToEvents()
        {
            Initialize();

            playerHealthEventChannel.Received().OnEventPublished += Arg.Any<EventChannelHandler<PlayerHealthEvent>>();
            scoreEventChannel.Received().OnEventPublished += Arg.Any<EventChannelHandler<ScoreEvent>>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();
            Destroy();

            playerHealthEventChannel.Received().OnEventPublished -= Arg.Any<EventChannelHandler<PlayerHealthEvent>>();
            scoreEventChannel.Received().OnEventPublished -= Arg.Any<EventChannelHandler<ScoreEvent>>();
        }

        [Test]
        public void OnHealthChangedUpdateHealthBarView()
        {
            Initialize();

            playerHealthEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<PlayerHealthEvent>>(
                new PlayerHealthEvent(health, OldHealthPoints, NewHealthPoints)
            );

            healthBarView.Received().SetHealthPercentage(0.7f);
        }

        [Test]
        public void OnScoreChangedUpdateScoreView()
        {
            Initialize();

            scoreEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<ScoreEvent>>(
                new ScoreEvent(score, OldScorePoints, NewScorePoints)
            );

            scoreView.Received().SetScore(70);
        }

        private void Initialize()
        {
            headUpMenuController.InjectHudController(healthBarView, scoreView, playerHealthEventChannel, scoreEventChannel);
            headUpMenuController.Awake();
        }

        private void Destroy()
        {
            headUpMenuController.OnDestroy();
        }
    }
}