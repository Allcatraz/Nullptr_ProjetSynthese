using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class KeyboardInputSensorTest : UnitTestCase
    {
        private UpEventHandler upEventHandler;
        private DownEventHandler downEventHandler;
        private ConfirmEventHandler confirmEventHandler;
        private TogglePauseEventHandler togglePauseEventHandler;
        private FireEventHandler fireEventHandler;
        private FowardEventHandler fowardEventHandler;
        private BackwardEventHandler backwardEventHandler;
        private RotateLeftEventHandler rotateLeftEventHandlerHandler;
        private RotateRightEventHandler rotateRightEventHandlerHandler;
        private IKeyboardInput input;
        private KeyboardInputSensor keyboardInputSensor;

        [SetUp]
        public void Before()
        {
            upEventHandler = CreateSubstitute<UpEventHandler>();
            downEventHandler = CreateSubstitute<DownEventHandler>();
            confirmEventHandler = CreateSubstitute<ConfirmEventHandler>();
            togglePauseEventHandler = CreateSubstitute<TogglePauseEventHandler>();
            fireEventHandler = CreateSubstitute<FireEventHandler>();
            fowardEventHandler = CreateSubstitute<FowardEventHandler>();
            backwardEventHandler = CreateSubstitute<BackwardEventHandler>();
            rotateLeftEventHandlerHandler = CreateSubstitute<RotateLeftEventHandler>();
            rotateRightEventHandlerHandler = CreateSubstitute<RotateRightEventHandler>();
            input = CreateSubstitute<IKeyboardInput>();
            keyboardInputSensor = CreateBehaviour<KeyboardInputSensor>();
        }

        [Test]
        public void WhenPressUpKeyFireUpEvent()
        {
            Initialize();

            PressKey(KeyCode.UpArrow);

            upEventHandler.Received()();
        }

        [Test]
        public void WhenPressDownKeyFireDownEvent()
        {
            Initialize();

            PressKey(KeyCode.DownArrow);

            downEventHandler.Received()();
        }

        [Test]
        public void WhenPressReturnKeyFireConfirmEvent()
        {
            Initialize();

            PressKey(KeyCode.Return);

            confirmEventHandler.Received()();
        }

        [Test]
        public void WhenPressEscapeKeyFirePauseEvent()
        {
            Initialize();

            PressKey(KeyCode.Escape);

            togglePauseEventHandler.Received()();
        }

        [Test]
        public void WhenPressSpaceKeyTriggerFireEvent()
        {
            Initialize();

            PressKey(KeyCode.Space);

            fireEventHandler.Received()();
        }

        [Test]
        public void WhenPressUpKeyFireFowardEvent()
        {
            Initialize();

            HoldKey(KeyCode.UpArrow);

            fowardEventHandler.Received()();
        }

        [Test]
        public void WhenPressDownKeyFireBackwardEvent()
        {
            Initialize();

            HoldKey(KeyCode.DownArrow);

            backwardEventHandler.Received()();
        }

        [Test]
        public void WhenPressLeftKeyFireRotateLeftEvent()
        {
            Initialize();

            HoldKey(KeyCode.LeftArrow);

            rotateLeftEventHandlerHandler.Received()();
        }

        [Test]
        public void WhenPressRightKeyFireRotateRightEvent()
        {
            Initialize();

            HoldKey(KeyCode.RightArrow);

            rotateRightEventHandlerHandler.Received()();
        }

        private void Initialize()
        {
            keyboardInputSensor.InjectKeyboardInputDevice(input);
            keyboardInputSensor.Awake();

            keyboardInputSensor.Keyboards.OnUp += upEventHandler;
            keyboardInputSensor.Keyboards.OnDown += downEventHandler;
            keyboardInputSensor.Keyboards.OnConfirm += confirmEventHandler;

            keyboardInputSensor.Keyboards.OnTogglePause += togglePauseEventHandler;
            keyboardInputSensor.Keyboards.OnFire += fireEventHandler;
            keyboardInputSensor.Keyboards.OnFoward += fowardEventHandler;
            keyboardInputSensor.Keyboards.OnBackward += backwardEventHandler;
            keyboardInputSensor.Keyboards.OnRotateLeft += rotateLeftEventHandlerHandler;
            keyboardInputSensor.Keyboards.OnRotateRight += rotateRightEventHandlerHandler;
        }

        private void HoldKey(KeyCode key)
        {
            input.GetKey(key).Returns(true);
            keyboardInputSensor.Update();
        }

        private void PressKey(KeyCode key)
        {
            input.GetKeyDown(key).Returns(true);
            keyboardInputSensor.Update();
        }
    }
}