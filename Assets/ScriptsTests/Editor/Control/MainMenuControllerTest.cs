using Harmony;
using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class MainMenuControllerTest : UnitTestCase
    {
        private UnityActivity gameActivity;
        private UnityMenu highScoresMenu;
        private ISelectable playButton;
        private IActivityStack activityStack;
        private IMenuStack menuStack;
        private MainMenuController mainMenuController;

        [SetUp]
        public void Before()
        {
            gameActivity = CreateSubstitute<UnityActivity>();
            highScoresMenu = CreateSubstitute<UnityMenu>();
            playButton = CreateSubstitute<ISelectable>();
            activityStack = CreateSubstitute<IActivityStack>();
            menuStack = CreateSubstitute<IMenuStack>();
            mainMenuController = CreateBehaviour<MainMenuController>();
        }

        [Test]
        public void WhenResumedPlayButtonIsSelected()
        {
            Initialize();

            CheckButtonSelected(playButton);
        }

        [Test]
        public void OnPlayButtonPressedStartGameActivity()
        {
            Initialize();

            mainMenuController.StartGame();

            CheckeGameActivityStarted();
        }

        [Test]
        public void OnScoresButtonPressedStartHighScoresMenu()
        {
            Initialize();

            mainMenuController.ShowHighScores();

            CheckeHighScoresMenuStarted();
        }

        [Test]
        public void OnQuitButtonPressedStopActivity()
        {
            Initialize();

            mainMenuController.QuitGame();

            CheckActivityStop();
        }

        private void Initialize()
        {
            mainMenuController.InjectMainMenuController(gameActivity,
                                                        highScoresMenu,
                                                        playButton,
                                                        activityStack,
                                                        menuStack);
            mainMenuController.Awake();
            mainMenuController.OnCreate();
            mainMenuController.OnResume();
        }

        private void CheckeGameActivityStarted()
        {
            activityStack.Received().StartActivity(gameActivity);
        }

        private void CheckeHighScoresMenuStarted()
        {
            menuStack.Received().StartMenu(highScoresMenu);
        }

        private void CheckActivityStop()
        {
            activityStack.Received().StopCurrentActivity();
        }

        private void CheckButtonSelected(ISelectable selectable)
        {
            selectable.Received().Select();
        }
    }
}