using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class LoadingScreenControllerTest : UnitTestCase
    {
        private ICanvas loadingScreenCanvas;
        private IActivityStack activityStack;
        private LoadingScreenController loadingScreenController;

        [SetUp]
        public void Before()
        {
            loadingScreenCanvas = CreateSubstitute<ICanvas>();
            activityStack = CreateSubstitute<IActivityStack>();
            loadingScreenController = CreateBehaviour<LoadingScreenController>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            activityStack.Received().OnActivityLoadingStarted += Arg.Any<ActivityLoadingEventHandler>();
            activityStack.Received().OnActivityLoadingEnded += Arg.Any<ActivityLoadingEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            activityStack.Received().OnActivityLoadingStarted -= Arg.Any<ActivityLoadingEventHandler>();
            activityStack.Received().OnActivityLoadingEnded -= Arg.Any<ActivityLoadingEventHandler>();
        }

        [Test]
        public void OnAwakeHideLoadingScreen()
        {
            Initialize();

            CheckLoadingScreenCanvasHiddenAtStart();
        }

        [Test]
        public void OnActivityLoadStartShowLoadingScreen()
        {
            Initialize();

            MakeActivityLoadStart();

            CheckLoadingScreenCanvasShow();
        }

        [Test]
        public void OnActivityLoadEndHideLoadingScreen()
        {
            Initialize();

            MakeActivityLoadEnd();

            CheckLoadingScreenCanvasHidden();
        }

        private void Initialize()
        {
            loadingScreenController.InjectLoadingScreenController(loadingScreenCanvas, activityStack);
            loadingScreenController.Awake();
            loadingScreenController.OnEnable();
        }

        private void Destroy()
        {
            loadingScreenController.OnDisable();
        }

        private void MakeActivityLoadStart()
        {
            activityStack.OnActivityLoadingStarted += Raise.Event<ActivityLoadingEventHandler>();
        }

        private void MakeActivityLoadEnd()
        {
            activityStack.OnActivityLoadingEnded += Raise.Event<ActivityLoadingEventHandler>();
        }

        private void CheckLoadingScreenCanvasShow()
        {
            loadingScreenCanvas.Received().Enabled = true;
        }

        private void CheckLoadingScreenCanvasHiddenAtStart()
        {
            loadingScreenCanvas.Received(1).Enabled = false;
        }

        private void CheckLoadingScreenCanvasHidden()
        {
            loadingScreenCanvas.Received(2).Enabled = false;
        }
    }
}