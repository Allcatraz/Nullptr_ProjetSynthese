using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Sensor/PlayerInputSensor")]
    public class PlayerInputSensor : InputSensor
    {
        private KeyboardInputSensor keyboardInputSensor;
        private GamePadInputSensor gamePadInputSensor;

        private PlayersInputDevice playersInputDevice;

        public IInputDevice Players
        {
            get { return playersInputDevice; }
        }

        private void InjectGameInputDevice([ApplicationScope] KeyboardInputSensor keyboardInputSensor,
                                          [ApplicationScope] GamePadInputSensor gamePadInputSensor)
        {
            this.keyboardInputSensor = keyboardInputSensor;
            this.gamePadInputSensor = gamePadInputSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectGameInputDevice");

            playersInputDevice = new PlayersInputDevice(keyboardInputSensor, gamePadInputSensor);
        }

        private void LateUpdate()
        {
            playersInputDevice.Reset();
        }

        private class PlayerInputDevice : TriggerOncePerFrameInputDevice
        {
            // ReSharper is wrong about this warning.
            // ReSharper disable once MemberCanBeProtected.Local
            public PlayerInputDevice(KeyboardInputSensor keyboardInputSensor, GamePadInputSensor gamePadInputSensor)
                : this(keyboardInputSensor.Keyboards, gamePadInputSensor.GamePads)
            {
            }

            // ReSharper is wrong about this warning.
            // ReSharper disable once MemberCanBeProtected.Local
            public PlayerInputDevice(IInputDevice keyboardInputDevice, IInputDevice gamePadInputDevice)
            {
                keyboardInputDevice.OnUp += NotifyUp;
                keyboardInputDevice.OnDown += NotifyDown;
                keyboardInputDevice.OnConfirm += NotifyConfirm;
                keyboardInputDevice.OnTogglePause += NotifyTogglePause;
                keyboardInputDevice.OnFire += NotifyFire;
                keyboardInputDevice.OnFoward += NotifyFoward;
                keyboardInputDevice.OnBackward += NotifyBackward;
                keyboardInputDevice.OnRotateLeft += NotifyRotateLeft;
                keyboardInputDevice.OnRotateRight += NotifyRotateRight;

                gamePadInputDevice.OnUp += NotifyUp;
                gamePadInputDevice.OnDown += NotifyDown;
                gamePadInputDevice.OnConfirm += NotifyConfirm;
                gamePadInputDevice.OnTogglePause += NotifyTogglePause;
                gamePadInputDevice.OnFire += NotifyFire;
                gamePadInputDevice.OnFoward += NotifyFoward;
                gamePadInputDevice.OnBackward += NotifyBackward;
                gamePadInputDevice.OnRotateLeft += NotifyRotateLeft;
                gamePadInputDevice.OnRotateRight += NotifyRotateRight;
            }

            public override IInputDevice this[int deviceIndex]
            {
                get { return this; }
            }
        }

        private class PlayersInputDevice : PlayerInputDevice
        {
            private readonly PlayerInputDevice[] players;

            public PlayersInputDevice(KeyboardInputSensor keyboardInputSensor, GamePadInputSensor gamePadInputSensor) :
                base(keyboardInputSensor, gamePadInputSensor)
            {
                players = new[]
                {
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[0], gamePadInputSensor.GamePads[0]),
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[1], gamePadInputSensor.GamePads[1]),
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[2], gamePadInputSensor.GamePads[2]),
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[3], gamePadInputSensor.GamePads[3])
                };
            }

            public override IInputDevice this[int deviceIndex]
            {
                get { return players[deviceIndex]; }
            }
			
			public override void Reset()
			{
				base.Reset();
				
				players[0].Reset();
				players[1].Reset();
				players[2].Reset();
				players[3].Reset();
			}
        }
    }
}