using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Sensor/KeyboardInputSensor")]
    public class KeyboardInputSensor : InputSensor
    {
        private Keyboard keyboard;

        private KeyboardsInputDevice keyboardsInputDevice;

        public IInputDevice Keyboards
        {
            get { return keyboardsInputDevice; }
        }

        private void InjectKeyboardInputDevice([ApplicationScope] Keyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        private void Awake()
        {
            InjectDependencies("InjectKeyboardInputDevice");

            keyboardsInputDevice = new KeyboardsInputDevice(keyboard);
        }

        private void Update()
        {
            keyboardsInputDevice.Update();
        }

        private class KeyboardsInputDevice : InputDevice
        {
            private readonly Keyboard keyboard;

            public KeyboardsInputDevice(Keyboard keyboard)
            {
                this.keyboard = keyboard;
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
                if (keyboard.GetKeyDown(KeyCode.UpArrow))
                {
                    NotifyUp();
                }
                if (keyboard.GetKeyDown(KeyCode.DownArrow))
                {
                    NotifyDown();
                }
                if (keyboard.GetKeyDown(KeyCode.Return))
                {
                    NotifyConfirm();
                }
            }

            private void HandleActionInput()
            {
                if (keyboard.GetKeyDown(KeyCode.Space))
                {
                    NotifyFire();
                }
                if (keyboard.GetKeyDown(KeyCode.Escape))
                {
                    NotifyTogglePause();
                }
            }

            private void HandleDirectionInput()
            {
                if (keyboard.GetKey(KeyCode.UpArrow))
                {
                    NotifyFoward();
                }
                if (keyboard.GetKey(KeyCode.DownArrow))
                {
                    NotifyBackward();
                }
            }

            private void HandleRotationInput()
            {
                if (keyboard.GetKey(KeyCode.LeftArrow))
                {
                    NotifyRotateLeft();
                }
                if (keyboard.GetKey(KeyCode.RightArrow))
                {
                    NotifyRotateRight();
                }
            }
        }
    }
}