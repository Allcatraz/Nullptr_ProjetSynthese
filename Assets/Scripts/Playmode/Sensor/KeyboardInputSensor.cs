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

        private void InjectKeyboardInputSensor([GameObjectScope] Keyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        private void Awake()
        {
            InjectDependencies("InjectKeyboardInputSensor");
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
                if (keyboard.GetKeyDown(KeyCode.Tab))
                {
                    NotifyToggleInventory();
                }
                if (keyboard.GetKeyDown(KeyCode.M))
                {
                    NotifyToggleMap();
                }
                if (keyboard.GetKeyDown(KeyCode.Escape))
                {
                    NotifyTogglePause();
                }
                if (keyboard.GetKeyDown(KeyCode.LeftShift))
                {
                    NotifySwitchSprintOn();
                }
                if (keyboard.GetKeyUp(KeyCode.LeftShift))
                {
                    NotifySwitchSprintOff();
                }
                if (keyboard.GetKeyDown(KeyCode.Alpha1))
                {
                    NotifySwitchPrimaryWeapon();
                }
                if (keyboard.GetKeyDown(KeyCode.Alpha2))
                {
                    NotifySwitchSecondaryWeapon();
                }
                if (keyboard.GetKeyDown(KeyCode.Alpha3))
                {
                    NotifySwitchThridWeapon();
                }
                if (keyboard.GetKeyDown(KeyCode.F))
                {
                    NotifyPickup();
                }
                if(keyboard.GetKeyDown(KeyCode.R))
                {
                    NotifyReload();
                }
                if (keyboard.GetKeyDown(KeyCode.F9))
                {
                    NotifyChangeViewMode();
                }
            }

            private void HandleDirectionInput()
            {
                if (keyboard.GetKey(KeyCode.W))
                {
                    NotifyMove(KeyCode.W);
                }
                if (keyboard.GetKey(KeyCode.A))
                {
                    NotifyMove(KeyCode.A);
                }
                if (keyboard.GetKey(KeyCode.S))
                {
                    NotifyMove(KeyCode.S);
                }
                if (keyboard.GetKey(KeyCode.D))
                {
                    NotifyMove(KeyCode.D);
                }
            }
        }
    }
}