using UnityEngine;
using Harmony;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Input/KeyboardInputSensor")]
    public class KeyboardInputSensor : InputSensor
    {
        private IKeyboardInput keyboardInput;

        private KeyboardsInputDevice keyboardsInputDevice;

        public virtual IInputDevice Keyboards
        {
            get { return keyboardsInputDevice; }
        }

        public void InjectKeyboardInputDevice([ApplicationScope] IKeyboardInput keyboardInput)
        {
            this.keyboardInput = keyboardInput;
        }

        public void Awake()
        {
            InjectDependencies("InjectKeyboardInputDevice");

            keyboardsInputDevice = new KeyboardsInputDevice(keyboardInput);
        }

        public void Update()
        {
            keyboardsInputDevice.Update();
        }

        private class KeyboardsInputDevice : InputDevice
        {
            private readonly IKeyboardInput keyboardInput;

            public KeyboardsInputDevice(IKeyboardInput keyboardInput)
            {
                this.keyboardInput = keyboardInput;
            }

            public void Update()
            {
                HandleUiInput();
                HandleActionInput();
                HandleDirectionInput();
                HandleRotationInput();
            }

            public override IInputDevice this[int deviceIndex]
            {
                get { return this; }
            }

            private void HandleUiInput()
            {
                if (keyboardInput.GetKeyDown(KeyCode.UpArrow))
                {
                    NotifyUp();
                }
                if (keyboardInput.GetKeyDown(KeyCode.DownArrow))
                {
                    NotifyDown();
                }
                if (keyboardInput.GetKeyDown(KeyCode.Return))
                {
                    NotifyConfirm();
                }
            }

            private void HandleActionInput()
            {
                if (keyboardInput.GetKeyDown(KeyCode.Space))
                {
                    NotifyFire();
                }
                if (keyboardInput.GetKeyDown(KeyCode.Escape))
                {
                    NotifyTogglePause();
                }
            }

            private void HandleDirectionInput()
            {
                if (keyboardInput.GetKey(KeyCode.UpArrow))
                {
                    NotifyFoward();
                }
                if (keyboardInput.GetKey(KeyCode.DownArrow))
                {
                    NotifyBackward();
                }
            }

            private void HandleRotationInput()
            {
                if (keyboardInput.GetKey(KeyCode.LeftArrow))
                {
                    NotifyRotateLeft();
                }
                if (keyboardInput.GetKey(KeyCode.RightArrow))
                {
                    NotifyRotateRight();
                }
            }
        }
    }
}