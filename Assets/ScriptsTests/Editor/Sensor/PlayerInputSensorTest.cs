using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using XInputDotNetPure;

namespace ProjetSynthese
{
    public class PlayerInputSensorTest : UnitTestCase
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

        private KeyboardInputSensor keyboardInput;
        private GamePadInputSensor gamePadInput;
        private PlayerInputSensor playerInputSensor;

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
            keyboardInput = CreateSubstitute<KeyboardInputSensor>();
            gamePadInput = CreateSubstitute<GamePadInputSensor>();
            playerInputSensor = CreateBehaviour<PlayerInputSensor>();
        }

        [Test]
        public void RedirectKeyboardUp()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnUp += upEventHandler,
                               () => playerInputSensor.Players.OnUp -= upEventHandler,
                               UpFromAllKeyboard,
                               () => upEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnUp += upEventHandler,
                            i => playerInputSensor.Players[(int) i].OnUp -= upEventHandler,
                            UpFromKeyboard,
                            i => upEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadUp()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnUp += upEventHandler,
                               () => playerInputSensor.Players.OnUp -= upEventHandler,
                               UpFromAllGamepad,
                               () => upEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnUp += upEventHandler,
                            i => playerInputSensor.Players[(int) i].OnUp -= upEventHandler,
                            UpFromGamepad,
                            i => upEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardDown()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnDown += downEventHandler,
                               () => playerInputSensor.Players.OnDown -= downEventHandler,
                               DownFromAllKeyboard,
                               () => downEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnDown += downEventHandler,
                            i => playerInputSensor.Players[(int) i].OnDown -= downEventHandler,
                            DownFromKeyboard,
                            i => downEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadDown()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnDown += downEventHandler,
                               () => playerInputSensor.Players.OnDown -= downEventHandler,
                               DownFromAllGamepad,
                               () => downEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnDown += downEventHandler,
                            i => playerInputSensor.Players[(int) i].OnDown -= downEventHandler,
                            DownFromGamepad,
                            i => downEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardConfirm()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnConfirm += confirmEventHandler,
                               () => playerInputSensor.Players.OnConfirm -= confirmEventHandler,
                               ConfirmFromAllKeyboard,
                               () => confirmEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnConfirm += confirmEventHandler,
                            i => playerInputSensor.Players[(int) i].OnConfirm -= confirmEventHandler,
                            ConfirmFromKeyboard,
                            i => confirmEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadConfirm()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnConfirm += confirmEventHandler,
                               () => playerInputSensor.Players.OnConfirm -= confirmEventHandler,
                               ConfirmFromAllGamepad,
                               () => confirmEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnConfirm += confirmEventHandler,
                            i => playerInputSensor.Players[(int) i].OnConfirm -= confirmEventHandler,
                            ConfirmFromGamepad,
                            i => confirmEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardPause()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnTogglePause += togglePauseEventHandler,
                               () => playerInputSensor.Players.OnTogglePause -= togglePauseEventHandler,
                               TogglePauseFromAllKeyboard,
                               () => togglePauseEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnTogglePause += togglePauseEventHandler,
                            i => playerInputSensor.Players[(int) i].OnTogglePause -= togglePauseEventHandler,
                            TogglePauseFromKeyboard,
                            i => togglePauseEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadPause()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnTogglePause += togglePauseEventHandler,
                               () => playerInputSensor.Players.OnTogglePause -= togglePauseEventHandler,
                               TogglePauseFromAllGamepad,
                               () => togglePauseEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnTogglePause += togglePauseEventHandler,
                            i => playerInputSensor.Players[(int) i].OnTogglePause -= togglePauseEventHandler,
                            TogglePauseFromGamepad,
                            i => togglePauseEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardFire()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnFire += fireEventHandler,
                               () => playerInputSensor.Players.OnFire -= fireEventHandler,
                               FireFromAllKeyboard,
                               () => fireEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnFire += fireEventHandler,
                            i => playerInputSensor.Players[(int) i].OnFire -= fireEventHandler,
                            FireFromKeyboard,
                            i => fireEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadFire()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnFire += fireEventHandler,
                               () => playerInputSensor.Players.OnFire -= fireEventHandler,
                               FireFromAllGamepad,
                               () => fireEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnFire += fireEventHandler,
                            i => playerInputSensor.Players[(int) i].OnFire -= fireEventHandler,
                            FireFromGamepad,
                            i => fireEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardFoward()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnFoward += fowardEventHandler,
                               () => playerInputSensor.Players.OnFoward -= fowardEventHandler,
                               FowardFromAllKeyboard,
                               () => fowardEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnFoward += fowardEventHandler,
                            i => playerInputSensor.Players[(int) i].OnFoward -= fowardEventHandler,
                            FowardFromKeyboard,
                            i => fowardEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadFoward()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnFoward += fowardEventHandler,
                               () => playerInputSensor.Players.OnFoward -= fowardEventHandler,
                               FowardFromAllGamepad,
                               () => fowardEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnFoward += fowardEventHandler,
                            i => playerInputSensor.Players[(int) i].OnFoward -= fowardEventHandler,
                            FowardFromGamepad,
                            i => fowardEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardBackward()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnBackward += backwardEventHandler,
                               () => playerInputSensor.Players.OnBackward -= backwardEventHandler,
                               BackwardFromAllKeyboard,
                               () => backwardEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnBackward += backwardEventHandler,
                            i => playerInputSensor.Players[(int) i].OnBackward -= backwardEventHandler,
                            BackwardFromKeyboard,
                            i => backwardEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadBackward()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnBackward += backwardEventHandler,
                               () => playerInputSensor.Players.OnBackward -= backwardEventHandler,
                               BackwardFromAllGamepad,
                               () => backwardEventHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnBackward += backwardEventHandler,
                            i => playerInputSensor.Players[(int) i].OnBackward -= backwardEventHandler,
                            BackwardFromGamepad,
                            i => backwardEventHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardRotateLeft()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnRotateLeft += rotateLeftEventHandlerHandler,
                               () => playerInputSensor.Players.OnRotateLeft -= rotateLeftEventHandlerHandler,
                               RotateLeftFromAllKeyboard,
                               () => rotateLeftEventHandlerHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnRotateLeft += rotateLeftEventHandlerHandler,
                            i => playerInputSensor.Players[(int) i].OnRotateLeft -= rotateLeftEventHandlerHandler,
                            RotateLeftFromKeyboard,
                            i => rotateLeftEventHandlerHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadRotateLeft()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnRotateLeft += rotateLeftEventHandlerHandler,
                               () => playerInputSensor.Players.OnRotateLeft -= rotateLeftEventHandlerHandler,
                               RotateLeftFromAllGamepad,
                               () => rotateLeftEventHandlerHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnRotateLeft += rotateLeftEventHandlerHandler,
                            i => playerInputSensor.Players[(int) i].OnRotateLeft -= rotateLeftEventHandlerHandler,
                            RotateLeftFromGamepad,
                            i => rotateLeftEventHandlerHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectKeyboardRotateRight()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnRotateRight += rotateRightEventHandlerHandler,
                               () => playerInputSensor.Players.OnRotateRight -= rotateRightEventHandlerHandler,
                               RotateRightFromAllKeyboard,
                               () => rotateRightEventHandlerHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnRotateRight += rotateRightEventHandlerHandler,
                            i => playerInputSensor.Players[(int) i].OnRotateRight -= rotateRightEventHandlerHandler,
                            RotateRightFromKeyboard,
                            i => rotateRightEventHandlerHandler.Received((int) i + 2)());
        }

        [Test]
        public void RedirectGamepadRotateRight()
        {
            Initialize();

            CheckPressAllEvent(() => playerInputSensor.Players.OnRotateRight += rotateRightEventHandlerHandler,
                               () => playerInputSensor.Players.OnRotateRight -= rotateRightEventHandlerHandler,
                               RotateRightFromAllGamepad,
                               () => rotateRightEventHandlerHandler.Received(1)());

            CheckPressEvent(i => playerInputSensor.Players[(int) i].OnRotateRight += rotateRightEventHandlerHandler,
                            i => playerInputSensor.Players[(int) i].OnRotateRight -= rotateRightEventHandlerHandler,
                            RotateRightFromGamepad,
                            i => rotateRightEventHandlerHandler.Received((int) i + 2)());
        }

        private void Initialize()
        {
            playerInputSensor.InjectGameInputDevice(keyboardInput, gamePadInput);
            playerInputSensor.Awake();
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

        private void UpFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnUp += Raise.Event<UpEventHandler>();
        }

        private void UpFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnUp += Raise.Event<UpEventHandler>();
        }

        private void DownFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnDown += Raise.Event<DownEventHandler>();
        }

        private void DownFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnDown += Raise.Event<DownEventHandler>();
        }

        private void ConfirmFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnConfirm += Raise.Event<ConfirmEventHandler>();
        }

        private void ConfirmFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnConfirm += Raise.Event<ConfirmEventHandler>();
        }

        private void TogglePauseFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnTogglePause += Raise.Event<TogglePauseEventHandler>();
        }

        private void TogglePauseFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnTogglePause += Raise.Event<TogglePauseEventHandler>();
        }

        private void FireFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnFire += Raise.Event<FireEventHandler>();
        }

        private void FireFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnFire += Raise.Event<FireEventHandler>();
        }

        private void FowardFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnFoward += Raise.Event<FowardEventHandler>();
        }

        private void FowardFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnFoward += Raise.Event<FowardEventHandler>();
        }

        private void BackwardFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnBackward += Raise.Event<BackwardEventHandler>();
        }

        private void BackwardFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnBackward += Raise.Event<BackwardEventHandler>();
        }

        private void RotateLeftFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnRotateLeft += Raise.Event<RotateLeftEventHandler>();
        }

        private void RotateLeftFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnRotateLeft += Raise.Event<RotateLeftEventHandler>();
        }

        private void RotateRightFromKeyboard(PlayerIndex playerIndex)
        {
            keyboardInput.Keyboards[(int) playerIndex].OnRotateRight += Raise.Event<RotateRightEventHandler>();
        }

        private void RotateRightFromAllKeyboard()
        {
            keyboardInput.Keyboards.OnRotateRight += Raise.Event<RotateRightEventHandler>();
        }

        private void UpFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnUp += Raise.Event<UpEventHandler>();
        }

        private void UpFromAllGamepad()
        {
            gamePadInput.GamePads.OnUp += Raise.Event<UpEventHandler>();
        }

        private void DownFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnDown += Raise.Event<DownEventHandler>();
        }

        private void DownFromAllGamepad()
        {
            gamePadInput.GamePads.OnDown += Raise.Event<DownEventHandler>();
        }

        private void ConfirmFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnConfirm += Raise.Event<ConfirmEventHandler>();
        }

        private void ConfirmFromAllGamepad()
        {
            gamePadInput.GamePads.OnConfirm += Raise.Event<ConfirmEventHandler>();
        }

        private void TogglePauseFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnTogglePause += Raise.Event<TogglePauseEventHandler>();
        }

        private void TogglePauseFromAllGamepad()
        {
            gamePadInput.GamePads.OnTogglePause += Raise.Event<TogglePauseEventHandler>();
        }

        private void FireFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnFire += Raise.Event<FireEventHandler>();
        }

        private void FireFromAllGamepad()
        {
            gamePadInput.GamePads.OnFire += Raise.Event<FireEventHandler>();
        }

        private void FowardFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnFoward += Raise.Event<FowardEventHandler>();
        }

        private void FowardFromAllGamepad()
        {
            gamePadInput.GamePads.OnFoward += Raise.Event<FowardEventHandler>();
        }

        private void BackwardFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnBackward += Raise.Event<BackwardEventHandler>();
        }

        private void BackwardFromAllGamepad()
        {
            gamePadInput.GamePads.OnBackward += Raise.Event<BackwardEventHandler>();
        }

        private void RotateLeftFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnRotateLeft += Raise.Event<RotateLeftEventHandler>();
        }

        private void RotateLeftFromAllGamepad()
        {
            gamePadInput.GamePads.OnRotateLeft += Raise.Event<RotateLeftEventHandler>();
        }

        private void RotateRightFromGamepad(PlayerIndex playerIndex)
        {
            gamePadInput.GamePads[(int) playerIndex].OnRotateRight += Raise.Event<RotateRightEventHandler>();
        }

        private void RotateRightFromAllGamepad()
        {
            gamePadInput.GamePads.OnRotateRight += Raise.Event<RotateRightEventHandler>();
        }
    }
}