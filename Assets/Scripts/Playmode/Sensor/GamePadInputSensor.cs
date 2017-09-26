using Harmony;
using UnityEngine;
using XInputDotNetPure;
using GamePad = Harmony.GamePad;
using GamePadState = Harmony.GamePadState;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Sensor/GamePadInputSensor")]
    public class GamePadInputSensor : InputSensor
    {
        private GamePad gamePad;

        private GamePadsInputDevice gamePadsInputDevice;

        public IInputDevice GamePads
        {
            get { return gamePadsInputDevice; }
        }

        private void InjectGamePadInputDevice([ApplicationScope] GamePad gamePad)
        {
            this.gamePad = gamePad;
        }

        private void Awake()
        {
            InjectDependencies("InjectGamePadInputDevice");

            gamePadsInputDevice = new GamePadsInputDevice(gamePad);
        }

        private void Update()
        {
            gamePadsInputDevice.Update();
        }

        private class GamePadInputDevice : InputDevice
        {
            private readonly GamePad gamePad;
            private readonly PlayerIndex playerIndex;
            private readonly bool isAllPlayers;

            private GamePadState currentFrameGamePadState;
            private GamePadState previousFrameGamePadState;

            // ReSharper is wrong about this warning.
            // ReSharper disable once MemberCanBeProtected.Local
            public GamePadInputDevice(GamePad gamePad) : this(gamePad, PlayerIndex.One, true)
            {
            }

            // ReSharper is wrong about this warning.
            // ReSharper disable once MemberCanBeProtected.Local
            public GamePadInputDevice(GamePad gamePad, PlayerIndex playerIndex) : this(gamePad, playerIndex, false)
            {
            }

            private GamePadInputDevice(GamePad gamePad, PlayerIndex playerIndex, bool isAllPlayers)
            {
                this.gamePad = gamePad;
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
                    NotifyMove(Vector3.up);
                }
                if (IsPressed(currentFrameGamePadState.DPad.Down))
                {
                    NotifyMove(Vector3.down);
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
                return isAllPlayers ? gamePad.GetGamepadState() : gamePad.GetGamepadState(playerIndex);
            }
        }

        private class GamePadsInputDevice : GamePadInputDevice
        {
            private readonly GamePadInputDevice[] gamePads;

            public GamePadsInputDevice(GamePad gamePad) : base(gamePad)
            {
                gamePads = new[]
                {
                    new GamePadInputDevice(gamePad, PlayerIndex.One),
                    new GamePadInputDevice(gamePad, PlayerIndex.Two),
                    new GamePadInputDevice(gamePad, PlayerIndex.Three),
                    new GamePadInputDevice(gamePad, PlayerIndex.Four)
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