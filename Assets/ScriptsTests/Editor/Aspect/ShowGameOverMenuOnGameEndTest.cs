using Harmony;
using Harmony.EventSystem;
using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class ShowGameOverMenuOnGameEndTest : UnitTestCase
    {
        private UnityMenu gameOverMenu;
        private GameEventChannel gameEventChannel;
        private IMenuStack menuStack;
        private ShowGameOverMenuOnGameEnd showGameOverMenuOnGameEnd;

        [SetUp]
        public void Before()
        {
            gameOverMenu = CreateSubstitute<UnityMenu>();
            gameEventChannel = CreateSubstitute<GameEventChannel>();
            menuStack = CreateSubstitute<IMenuStack>();
            showGameOverMenuOnGameEnd = CreateBehaviour<ShowGameOverMenuOnGameEnd>();
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
        public void ShowMenuOnGameEnd()
        {
            Initialize();

            MakeGameEnd();

            CheckMenuStarted();
        }

        private void Initialize()
        {
            showGameOverMenuOnGameEnd.InjectShowGameOverMenuOnGameEnd(gameOverMenu,
                                                                      menuStack,
                                                                      gameEventChannel);
            showGameOverMenuOnGameEnd.Awake();
            showGameOverMenuOnGameEnd.OnEnable();
        }

        private void Disable()
        {
            showGameOverMenuOnGameEnd.OnDisable();
        }

        private void MakeGameEnd()
        {
            gameEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<GameEvent>>(new GameEvent(true, new Score()));
        }

        private void CheckMenuStarted()
        {
            menuStack.Received().StartMenu(gameOverMenu, Arg.Any<GameEvent>());
        }
    }
}