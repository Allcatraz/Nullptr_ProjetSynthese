using Harmony.EventSystem;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DisableAddScoreOnGameEndTest : UnitTestCase
    {
        private AddScoreOnPrefabDeath addScoreOnPrefabDeath;
        private GameEventChannel gameEventChannel;
        private DisableAddScoreOnGameEnd disableAddScoreOnGameEnd;

        [SetUp]
        public void Before()
        {
            addScoreOnPrefabDeath = CreateSubstitute<AddScoreOnPrefabDeath>();
            gameEventChannel = CreateSubstitute<GameEventChannel>();
            disableAddScoreOnGameEnd = CreateBehaviour<DisableAddScoreOnGameEnd>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            gameEventChannel.Received().OnEventPublished += Arg.Any<EventChannelHandler<GameEvent>>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            gameEventChannel.Received().OnEventPublished -= Arg.Any<EventChannelHandler<GameEvent>>();
        }

        [Test]
        public void WhenGameEndisableScoreCount()
        {
            Initialize();

            MakeGameEnd();

            CheckDisabled();
        }

        private void Initialize()
        {
            disableAddScoreOnGameEnd.InjectDisableAddScoreOnPlayerDeath(addScoreOnPrefabDeath,
                                                                        gameEventChannel);
            disableAddScoreOnGameEnd.Awake();
            disableAddScoreOnGameEnd.OnEnable();
        }

        public void Disable()
        {
            disableAddScoreOnGameEnd.OnDisable();
        }

        private void MakeGameEnd()
        {
            gameEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<GameEvent>>(new GameEvent(true, new Score()));
        }

        private void CheckDisabled()
        {
            addScoreOnPrefabDeath.Received().DisableScoreCount();
        }
    }
}