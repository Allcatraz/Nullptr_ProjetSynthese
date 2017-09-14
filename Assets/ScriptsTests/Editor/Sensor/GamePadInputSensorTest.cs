using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using XInputDotNetPure;
using GamePadButtons = Harmony.GamePadButtons;
using GamePadState = Harmony.GamePadState;
using GamePadDPad = Harmony.GamePadDPad;
using GamePadTriggers = Harmony.GamePadTriggers;
using GamePadThumbSticks = Harmony.GamePadThumbSticks;

namespace ProjetSynthese
{
    public class GamePadInputSensorTest : UnitTestCase
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
        private IGamePadInput gamePadInput;
        private GamePadInputSensor gamePadInputSensor;

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
            gamePadInput = CreateSubstitute<IGamePadInput>();

            gamePadInputSensor = CreateBehaviour<GamePadInputSensor>();
        }

        [Test]
        public void WhenPressDPadUpFireUpEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnUp += upEventHandler,
                               () => gamePadInputSensor.GamePads.OnUp -= upEventHandler,
                               () => PressKey(up: ButtonState.Pressed),
                               () => upEventHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnUp += upEventHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnUp -= upEventHandler,
                            i => PressKey(i, up: ButtonState.Pressed),
                            i => upEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenPressDPadDownFireDownEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnDown += downEventHandler,
                               () => gamePadInputSensor.GamePads.OnDown -= downEventHandler,
                               () => PressKey(down: ButtonState.Pressed),
                               () => downEventHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnDown += downEventHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnDown -= downEventHandler,
                            i => PressKey(i, down: ButtonState.Pressed),
                            i => downEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenPressAButtonFireConfirmEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnConfirm += confirmEventHandler,
                               () => gamePadInputSensor.GamePads.OnConfirm -= confirmEventHandler,
                               () => PressKey(a: ButtonState.Pressed),
                               () => confirmEventHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnConfirm += confirmEventHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnConfirm -= confirmEventHandler,
                            i => PressKey(i, a: ButtonState.Pressed),
                            i => confirmEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenPressStartButtonFirePauseEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnTogglePause += togglePauseEventHandler,
                               () => gamePadInputSensor.GamePads.OnTogglePause -= togglePauseEventHandler,
                               () => PressKey(start: ButtonState.Pressed),
                               () => togglePauseEventHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnTogglePause += togglePauseEventHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnTogglePause -= togglePauseEventHandler,
                            i => PressKey(i, start: ButtonState.Pressed),
                            i => togglePauseEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenPressAButtonTriggerFireEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnFire += fireEventHandler,
                               () => gamePadInputSensor.GamePads.OnFire -= fireEventHandler,
                               () => PressKey(a: ButtonState.Pressed),
                               () => fireEventHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnFire += fireEventHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnFire -= fireEventHandler,
                            i => PressKey(i, a: ButtonState.Pressed),
                            i => fireEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenHoldDPadUpFireFowardEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnFoward += fowardEventHandler,
                               () => gamePadInputSensor.GamePads.OnFoward -= fowardEventHandler,
                               () => HoldKey(up: ButtonState.Pressed),
                               () => fowardEventHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnFoward += fowardEventHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnFoward -= fowardEventHandler,
                            i => HoldKey(i, up: ButtonState.Pressed),
                            i => fowardEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenHoldDPadDownFireBackwardEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnBackward += backwardEventHandler,
                               () => gamePadInputSensor.GamePads.OnBackward -= backwardEventHandler,
                               () => HoldKey(down: ButtonState.Pressed),
                               () => backwardEventHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnBackward += backwardEventHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnBackward -= backwardEventHandler,
                            i => HoldKey(i, down: ButtonState.Pressed),
                            i => backwardEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenHoldDPadLeftFireRotateLeftEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnRotateLeft += rotateLeftEventHandlerHandler,
                               () => gamePadInputSensor.GamePads.OnRotateLeft -= rotateLeftEventHandlerHandler,
                               () => HoldKey(left: ButtonState.Pressed),
                               () => rotateLeftEventHandlerHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnRotateLeft += rotateLeftEventHandlerHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnRotateLeft -= rotateLeftEventHandlerHandler,
                            i => HoldKey(i, left: ButtonState.Pressed),
                            i => rotateLeftEventHandlerHandler.Received((int) i + 2)());
        }

        [Test]
        public void WhenHoldDPadRightFireRotateRightEvent()
        {
            Initialize();

            CheckPressAllEvent(() => gamePadInputSensor.GamePads.OnRotateRight += rotateRightEventHandlerHandler,
                               () => gamePadInputSensor.GamePads.OnRotateRight -= rotateRightEventHandlerHandler,
                               () => HoldKey(right: ButtonState.Pressed),
                               () => rotateRightEventHandlerHandler.Received(1)());

            CheckPressEvent(i => gamePadInputSensor.GamePads[(int) i].OnRotateRight += rotateRightEventHandlerHandler,
                            i => gamePadInputSensor.GamePads[(int) i].OnRotateRight -= rotateRightEventHandlerHandler,
                            i => HoldKey(i, right: ButtonState.Pressed),
                            i => rotateRightEventHandlerHandler.Received((int) i + 2)());
        }

        private void Initialize()
        {
            gamePadInputSensor.InjectGamePadInputDevice(gamePadInput);
            gamePadInputSensor.Awake();
        }

        delegate void RegisterHandler(PlayerIndex playerIndex);
        delegate void PressKeyHandler(PlayerIndex playerIndex);
        delegate void VerifyHandler(PlayerIndex playerIndex);

        private void CheckPressEvent(RegisterHandler register,
                                     RegisterHandler unRegister,
                                     PressKeyHandler pressKey,
                                     VerifyHandler verify)
        {
            for (int i = 0; i < 4; i++)
            {
                register((PlayerIndex) i);
                pressKey((PlayerIndex) i);
                verify((PlayerIndex) i);
                unRegister((PlayerIndex) i);
            }
        }

        delegate void RegisterAllHandler();
        delegate void PressKeyAllHandler();
        delegate void VerifyAllHandler();

        private void CheckPressAllEvent(RegisterAllHandler register,
                                        RegisterAllHandler unRegister,
                                        PressKeyAllHandler pressKey,
                                        VerifyAllHandler verify)
        {
            register();
            pressKey();
            verify();
            unRegister();
        }

        private void CheckPressDownEvent(IInputDevice inputDevice, PressKeyAllHandler pressKey)
        {
            UpEventHandler eventHandler = CreateSubstitute<UpEventHandler>();
            inputDevice.OnUp += eventHandler;

            pressKey();

            eventHandler.Received()();
            inputDevice.OnUp -= eventHandler;
        }

        private void PressKey(ButtonState start = ButtonState.Released,
                              ButtonState back = ButtonState.Released,
                              ButtonState leftStick = ButtonState.Released,
                              ButtonState rightStick = ButtonState.Released,
                              ButtonState leftShoulder = ButtonState.Released,
                              ButtonState rightShoulder = ButtonState.Released,
                              ButtonState guide = ButtonState.Released,
                              ButtonState a = ButtonState.Released,
                              ButtonState b = ButtonState.Released,
                              ButtonState x = ButtonState.Released,
                              ButtonState y = ButtonState.Released,
                              ButtonState up = ButtonState.Released,
                              ButtonState down = ButtonState.Released,
                              ButtonState left = ButtonState.Released,
                              ButtonState right = ButtonState.Released)
        {
            gamePadInput.GetGamepadState().Returns(new GamePadState(true, //Previous state
                                                                    new GamePadButtons(ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released,
                                                                                       ButtonState.Released),
                                                                    new GamePadDPad(ButtonState.Released,
                                                                                    ButtonState.Released,
                                                                                    ButtonState.Released,
                                                                                    ButtonState.Released),
                                                                    new GamePadTriggers(0, 0),
                                                                    new GamePadThumbSticks(
                                                                        new GamePadThumbSticks.StickValue(0, 0),
                                                                        new GamePadThumbSticks.StickValue(0, 0))),
                                                   new GamePadState(true, //Current state
                                                                    new GamePadButtons(start,
                                                                                       back,
                                                                                       leftStick,
                                                                                       rightStick,
                                                                                       leftShoulder,
                                                                                       rightShoulder,
                                                                                       guide,
                                                                                       a,
                                                                                       b,
                                                                                       x,
                                                                                       y),
                                                                    new GamePadDPad(up,
                                                                                    down,
                                                                                    left,
                                                                                    right),
                                                                    new GamePadTriggers(0, 0),
                                                                    new GamePadThumbSticks(
                                                                        new GamePadThumbSticks.StickValue(0, 0),
                                                                        new GamePadThumbSticks.StickValue(0, 0))));
            gamePadInputSensor.Update(); //Update twice : 1 for previous state and 1 for current state
            gamePadInputSensor.Update();
        }

        private void PressKey(PlayerIndex playerIndex,
                              ButtonState start = ButtonState.Released,
                              ButtonState back = ButtonState.Released,
                              ButtonState leftStick = ButtonState.Released,
                              ButtonState rightStick = ButtonState.Released,
                              ButtonState leftShoulder = ButtonState.Released,
                              ButtonState rightShoulder = ButtonState.Released,
                              ButtonState guide = ButtonState.Released,
                              ButtonState a = ButtonState.Released,
                              ButtonState b = ButtonState.Released,
                              ButtonState x = ButtonState.Released,
                              ButtonState y = ButtonState.Released,
                              ButtonState up = ButtonState.Released,
                              ButtonState down = ButtonState.Released,
                              ButtonState left = ButtonState.Released,
                              ButtonState right = ButtonState.Released)
        {
            gamePadInput.GetGamepadState(playerIndex).Returns(new GamePadState(true, //Previous state
                                                                               new GamePadButtons(ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released,
                                                                                                  ButtonState.Released),
                                                                               new GamePadDPad(ButtonState.Released,
                                                                                               ButtonState.Released,
                                                                                               ButtonState.Released,
                                                                                               ButtonState.Released),
                                                                               new GamePadTriggers(0, 0),
                                                                               new GamePadThumbSticks(
                                                                                   new GamePadThumbSticks.StickValue(0, 0),
                                                                                   new GamePadThumbSticks.StickValue(0, 0))),
                                                              new GamePadState(true, //Current state
                                                                               new GamePadButtons(start,
                                                                                                  back,
                                                                                                  leftStick,
                                                                                                  rightStick,
                                                                                                  leftShoulder,
                                                                                                  rightShoulder,
                                                                                                  guide,
                                                                                                  a,
                                                                                                  b,
                                                                                                  x,
                                                                                                  y),
                                                                               new GamePadDPad(up,
                                                                                               down,
                                                                                               left,
                                                                                               right),
                                                                               new GamePadTriggers(0, 0),
                                                                               new GamePadThumbSticks(
                                                                                   new GamePadThumbSticks.StickValue(0, 0),
                                                                                   new GamePadThumbSticks.StickValue(0, 0))));
            gamePadInputSensor.Update(); //Update twice : 1 for previous state and 1 for current state
            gamePadInputSensor.Update();
        }

        private void HoldKey(ButtonState start = ButtonState.Released,
                             ButtonState back = ButtonState.Released,
                             ButtonState leftStick = ButtonState.Released,
                             ButtonState rightStick = ButtonState.Released,
                             ButtonState leftShoulder = ButtonState.Released,
                             ButtonState rightShoulder = ButtonState.Released,
                             ButtonState guide = ButtonState.Released,
                             ButtonState a = ButtonState.Released,
                             ButtonState b = ButtonState.Released,
                             ButtonState x = ButtonState.Released,
                             ButtonState y = ButtonState.Released,
                             ButtonState up = ButtonState.Released,
                             ButtonState down = ButtonState.Released,
                             ButtonState left = ButtonState.Released,
                             ButtonState right = ButtonState.Released)
        {
            gamePadInput.GetGamepadState().Returns(new GamePadState(true, //Previous state
                                                                    new GamePadButtons(start,
                                                                                       back,
                                                                                       leftStick,
                                                                                       rightStick,
                                                                                       leftShoulder,
                                                                                       rightShoulder,
                                                                                       guide,
                                                                                       a,
                                                                                       b,
                                                                                       x,
                                                                                       y),
                                                                    new GamePadDPad(up,
                                                                                    down,
                                                                                    left,
                                                                                    right),
                                                                    new GamePadTriggers(0, 0),
                                                                    new GamePadThumbSticks(
                                                                        new GamePadThumbSticks.StickValue(0, 0),
                                                                        new GamePadThumbSticks.StickValue(0, 0))),
                                                   new GamePadState(true, //Current state
                                                                    new GamePadButtons(start,
                                                                                       back,
                                                                                       leftStick,
                                                                                       rightStick,
                                                                                       leftShoulder,
                                                                                       rightShoulder,
                                                                                       guide,
                                                                                       a,
                                                                                       b,
                                                                                       x,
                                                                                       y),
                                                                    new GamePadDPad(up,
                                                                                    down,
                                                                                    left,
                                                                                    right),
                                                                    new GamePadTriggers(0, 0),
                                                                    new GamePadThumbSticks(
                                                                        new GamePadThumbSticks.StickValue(0, 0),
                                                                        new GamePadThumbSticks.StickValue(0, 0))));
            gamePadInputSensor.Update();
        }

        private void HoldKey(PlayerIndex playerIndex,
                             ButtonState start = ButtonState.Released,
                             ButtonState back = ButtonState.Released,
                             ButtonState leftStick = ButtonState.Released,
                             ButtonState rightStick = ButtonState.Released,
                             ButtonState leftShoulder = ButtonState.Released,
                             ButtonState rightShoulder = ButtonState.Released,
                             ButtonState guide = ButtonState.Released,
                             ButtonState a = ButtonState.Released,
                             ButtonState b = ButtonState.Released,
                             ButtonState x = ButtonState.Released,
                             ButtonState y = ButtonState.Released,
                             ButtonState up = ButtonState.Released,
                             ButtonState down = ButtonState.Released,
                             ButtonState left = ButtonState.Released,
                             ButtonState right = ButtonState.Released)
        {
            gamePadInput.GetGamepadState(playerIndex).Returns(new GamePadState(true, //Previous state
                                                                               new GamePadButtons(start,
                                                                                                  back,
                                                                                                  leftStick,
                                                                                                  rightStick,
                                                                                                  leftShoulder,
                                                                                                  rightShoulder,
                                                                                                  guide,
                                                                                                  a,
                                                                                                  b,
                                                                                                  x,
                                                                                                  y),
                                                                               new GamePadDPad(up,
                                                                                               down,
                                                                                               left,
                                                                                               right),
                                                                               new GamePadTriggers(0, 0),
                                                                               new GamePadThumbSticks(
                                                                                   new GamePadThumbSticks.StickValue(0, 0),
                                                                                   new GamePadThumbSticks.StickValue(0, 0))),
                                                              new GamePadState(true, //Current state
                                                                               new GamePadButtons(start,
                                                                                                  back,
                                                                                                  leftStick,
                                                                                                  rightStick,
                                                                                                  leftShoulder,
                                                                                                  rightShoulder,
                                                                                                  guide,
                                                                                                  a,
                                                                                                  b,
                                                                                                  x,
                                                                                                  y),
                                                                               new GamePadDPad(up,
                                                                                               down,
                                                                                               left,
                                                                                               right),
                                                                               new GamePadTriggers(0, 0),
                                                                               new GamePadThumbSticks(
                                                                                   new GamePadThumbSticks.StickValue(0, 0),
                                                                                   new GamePadThumbSticks.StickValue(0, 0))));
            gamePadInputSensor.Update();
        }
    }
}