using Harmony;
using Harmony.Testing;
using Harmony.Unity;
using NUnit.Framework;
using NSubstitute;

namespace ProjetSynthese
{
    public class MainActivityControllerTest : UnitTestCase
    {
        private UnityMenu mainMenu;
        private IMenuStack menuStack;
        private MainActivityController mainActivityController;

        [SetUp]
        public void Before()
        {
            mainMenu = CreateSubstitute<UnityMenu>();
            menuStack = CreateSubstitute<IMenuStack>();
            mainActivityController = CreateBehaviour<MainActivityController>();
        }

        [Test]
        public void WhenResumedStartMainMenu()
        {
            Initialize();

            HasStartedMainMenu();
        }

        private void Initialize()
        {
            mainActivityController.InjectMainActivityController(mainMenu,
                                                                menuStack);
            mainActivityController.Awake();
            mainActivityController.OnCreate();
        }

        private void HasStartedMainMenu()
        {
            menuStack.Received().StartMenu(mainMenu);
        }
    }
}