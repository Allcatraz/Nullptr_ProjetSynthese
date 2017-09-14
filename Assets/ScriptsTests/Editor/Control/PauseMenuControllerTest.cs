using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class PauseMenuControllerTest : UnitTestCase
    {
        private ISelectable resumeButton;
        private ITime time;
        private IActivityStack activityStack;
        private IMenuStack menuStack;
        private PauseMenuController pauseMenuController;

        [SetUp]
        public void Before()
        {
            resumeButton = CreateSubstitute<ISelectable>();
            time = CreateSubstitute<ITime>();
            activityStack = CreateSubstitute<IActivityStack>();
            menuStack = CreateSubstitute<IMenuStack>();
            pauseMenuController = CreateBehaviour<PauseMenuController>();
        }

        [Test]
        public void WhenResumeResumeButtonIsSelected()
        {
            Initialize();

            pauseMenuController.OnResume();

            CheckResumeButtonSelected();
        }

        [Test]
        public void WhenCreatedPauseTime()
        {
            Initialize();

            pauseMenuController.OnCreate(null);

            CheckTimeStopped();
        }

        [Test]
        public void WhenStopTimeIsResumed()
        {
            Initialize();

            pauseMenuController.OnStop();

            CheckTimeResumed();
        }

        [Test]
        public void WhenPressingResumeButtonPauseMenuIsClosed()
        {
            Initialize();

            pauseMenuController.ResumeGame();

            CheckCurrentMenuStoped();
        }

        [Test]
        public void WhenPressingQuitExitActivity()
        {
            Initialize();

            pauseMenuController.QuitGame();

            CheckCurrentActivityIsStopped();
        }

        private void Initialize()
        {
            pauseMenuController.InjectPauseController(resumeButton,
                                                  time,
                                                  activityStack,
                                                  menuStack);
            pauseMenuController.Awake();
        }

        private void CheckTimeStopped()
        {
            time.Received().Pause();
        }

        private void CheckTimeResumed()
        {
            time.Received().Resume();
        }

        private void CheckCurrentActivityIsStopped()
        {
            activityStack.Received().StopCurrentActivity();
        }

        private void CheckResumeButtonSelected()
        {
            resumeButton.Received().Select();
        }

        private void CheckCurrentMenuStoped()
        {
            menuStack.Received().StopCurrentMenu();
        }
    }
}