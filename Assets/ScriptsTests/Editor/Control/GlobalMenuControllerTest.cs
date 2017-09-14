using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class GlobalMenuControllerTest : UnitTestCase
    {
        private ISelectable button1;
        private ISelectable button2;
        private ISelectable button3;
        private PlayerInputSensor playerInputSensor;
        private IMenuState menuState;
        private IInputDevice playersInputDevice;
        private GlobalMenuController globalMenuController;

        [SetUp]
        public void Before()
        {
            button1 = CreateSubstitute<ISelectable>();
            button2 = CreateSubstitute<ISelectable>();
            button3 = CreateSubstitute<ISelectable>();

            button1.SelectNext().Returns(button2);
            button2.SelectNext().Returns(button3);
            button3.SelectNext().Returns(button1);

            button1.SelectPrevious().Returns(button3);
            button3.SelectPrevious().Returns(button2);
            button2.SelectPrevious().Returns(button1);

            playerInputSensor = CreateSubstitute<PlayerInputSensor>();
            menuState = CreateSubstitute<IMenuState>();
            playersInputDevice = CreateSubstitute<IInputDevice>();
            globalMenuController = CreateBehaviour<GlobalMenuController>();

            playerInputSensor.Players.Returns(playersInputDevice);
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            playersInputDevice.Received().OnUp += Arg.Any<UpEventHandler>();
            playersInputDevice.Received().OnDown += Arg.Any<DownEventHandler>();
            playersInputDevice.Received().OnConfirm += Arg.Any<ConfirmEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            playersInputDevice.Received().OnUp -= Arg.Any<UpEventHandler>();
            playersInputDevice.Received().OnDown -= Arg.Any<DownEventHandler>();
            playersInputDevice.Received().OnConfirm -= Arg.Any<ConfirmEventHandler>();
        }

        [Test]
        public void SupportsHavingNoSelectable()
        {
            Initialize();

            //This should not throw any exception. The test will fail if it does.
            PressUp();
            PressDown();
        }

        [Test]
        public void SubmitSelectedOnConfirm()
        {
            Initialize();

            SelectButton(button1);
            PressConfirm();

            CheckSubmit(button1);
        }

        [Test]
        public void SelectNextButtonOnDown()
        {
            Initialize();

            SelectButton(button1);
            PressDown();

            CheckSelectedNext(button1);
        }

        [Test]
        public void SelectPreviousButtonOnUp()
        {
            Initialize();

            SelectButton(button1);
            PressUp();

            CheckSelectedPrevious(button1);
        }

        private void Initialize()
        {
            globalMenuController.InjectGlobalMenuController(playerInputSensor, menuState);
            globalMenuController.Awake();
            globalMenuController.OnEnable();
        }

        private void Disable()
        {
            globalMenuController.OnDisable();
        }

        private void SelectButton(ISelectable selectable)
        {
            menuState.CurrentSelected.Returns(selectable);
        }

        private void PressUp()
        {
            playersInputDevice.OnUp += Raise.Event<UpEventHandler>();
        }

        private void PressDown()
        {
            playersInputDevice.OnDown += Raise.Event<DownEventHandler>();
        }

        private void PressConfirm()
        {
            playersInputDevice.OnConfirm += Raise.Event<ConfirmEventHandler>();
        }

        private void CheckSubmit(ISelectable selectable)
        {
            selectable.Received().Click();
        }

        private void CheckSelectedNext(ISelectable selectable)
        {
            selectable.Received().SelectNext();
        }

        private void CheckSelectedPrevious(ISelectable selectable)
        {
            selectable.Received().SelectPrevious();
        }
    }
}