using Harmony;
using Harmony.Injection;
using UnityEngine;
using XInputDotNetPure;
using GamePadState = Harmony.GamePadState;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Input/GamePadInputSensor")]
    public class GamePadInputSensor : InputSensor
    {
        private IGamePadInput gamePadInput;

        private GamePadsInputDevice gamePadsInputDevice;

        public virtual IInputDevice GamePads
        {
            get { return gamePadsInputDevice; }
        }

        public void InjectGamePadInputDevice([ApplicationScope] IGamePadInput gamePadInput)
        {
            this.gamePadInput = gamePadInput;
        }

        public void Awake()
        {
            InjectDependencies("InjectGamePadInputDevice");

            gamePadsInputDevice = new GamePadsInputDevice(gamePadInput);
        }

        public void Update()
        {
            gamePadsInputDevice.Update();
        }

        public IInputDevice GetGamePad(int playerIndex)
        {
            return gamePadsInputDevice[playerIndex];
        }

        private class GamePadInputDevice : InputDevice
        {
            private readonly IGamePadInput gamePadInput;
            private readonly PlayerIndex playerIndex;
            private readonly bool isAllPlayers;

            private GamePadState currentFrameGamePadState;
            private GamePadState previousFrameGamePadState;

            public GamePadInputDevice(IGamePadInput gamePadInput) : this(gamePadInput, PlayerIndex.One, true)
            {
            }

            public GamePadInputDevice(IGamePadInput gamePadInput, PlayerIndex playerIndex) : this(gamePadInput, playerIndex, false)
            {
            }

            private GamePadInputDevice(IGamePadInput gamePadInput, PlayerIndex playerIndex, bool isAllPlayers)
            {
                this.gamePadInput = gamePadInput;
                this.playerIndex = playerIndex;
                this.isAllPlayers = isAllPlayers;

                previousFrameGamePadState = GetCurrentGamePadState();
            }

            public override IInputDevice this[int deviceIndex]
            {
                get { return this; }
            }

            public virtual void Update()
            {
                currentFrameGamePadState = GetCurrentGamePadState();

                HandleUiInput();
                HandleActionInput();
                HandleDirectionInput();
                HandleRotationInput();

                previousFrameGamePadState = currentFrameGamePadState;
            }

            private void HandleUiInput()
            {
                if (IsPressedSinceLastFrame(previousFrameGamePadState.DPad.Up, currentFrameGamePadState.DPad.Up))
                {
                    NotifyUp();
                }
                if (IsPressedSinceLastFrame(previousFrameGamePadState.DPad.Down, currentFrameGamePadState.DPad.Down))
                {
                    NotifyDown();
                }
                if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.A, currentFrameGamePadState.Buttons.A))
                {
                    NotifyConfirm();
                }
            }

            private void HandleActionInput()
            {
                if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.A, currentFrameGamePadState.Buttons.A))
                {
                    NotifyFire();
                }
                if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.Start, currentFrameGamePadState.Buttons.Start))
                {
                    NotifyTogglePause();
                }
            }

            private void HandleDirectionInput()
            {
                if (IsPressed(currentFrameGamePadState.DPad.Up))
                {
                    NotifyFoward();
                }
                if (IsPressed(currentFrameGamePadState.DPad.Down))
                {
                    NotifyBackward();
                }
            }

            private void HandleRotationInput()
            {
                if (IsPressed(currentFrameGamePadState.DPad.Left))
                {
                    NotifyRotateLeft();
                }
                if (IsPressed(currentFrameGamePadState.DPad.Right))
                {
                    NotifyRotateRight();
                }
            }

            private bool IsPressed(ButtonState currentState)
            {
                return currentState == ButtonState.Pressed;
            }

            private bool IsPressedSinceLastFrame(ButtonState previousState, ButtonState currentState)
            {
                return previousState == ButtonState.Released && currentState == ButtonState.Pressed;
            }

            private GamePadState GetCurrentGamePadState()
            {
                return isAllPlayers ? gamePadInput.GetGamepadState() : gamePadInput.GetGamepadState(playerIndex);
            }
        }

        private class GamePadsInputDevice : GamePadInputDevice
        {
            private readonly GamePadInputDevice[] gamePads;

            public GamePadsInputDevice(IGamePadInput gamePadInput) : base(gamePadInput)
            {
                gamePads = new[]
                {
                    new GamePadInputDevice(gamePadInput, PlayerIndex.One),
                    new GamePadInputDevice(gamePadInput, PlayerIndex.Two),
                    new GamePadInputDevice(gamePadInput, PlayerIndex.Three),
                    new GamePadInputDevice(gamePadInput, PlayerIndex.Four)
                };
            }

            public override IInputDevice this[int deviceIndex]
            {
                get { return gamePads[deviceIndex]; }
            }

            public override void Update()
            {
                base.Update();

                foreach (GamePadInputDevice gamePadInputDevice in gamePads)
                {
                    gamePadInputDevice.Update();
                }
            }
        }
    }
}