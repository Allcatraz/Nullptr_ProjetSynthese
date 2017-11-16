using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Sensor/KeyboardInputSensor")]
    public class KeyboardInputSensor : InputSensor
    {
        private Keyboard keyboard;

        private KeyboardsInputDevice keyboardsInputDevice;

        public IInputDevice Keyboards { get { return keyboardsInputDevice; } }

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
                HandleActionInput();
                HandleDirectionInput();
            }

            public override IInputDevice this[int deviceIndex] { get { return this; } }

            private void HandleActionInput()
            {
                if (keyboard.GetKeyDown(ActionKey.Instance.ToggleInventory))
                {
                    NotifyToggleInventory();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.ToggleMap))
                {
                    NotifyToggleMap();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.TogglePause))
                {
                    NotifyTogglePause();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.ToggleSprint))
                {
                    NotifySwitchSprintOn();
                }
                if (keyboard.GetKeyUp(ActionKey.Instance.ToggleSprint))
                {
                    NotifySwitchSprintOff();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.SwitchToPrimaryWeapon))
                {
                    NotifySwitchPrimaryWeapon();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.SwitchToSecondaryWeapon))
                {
                    NotifySwitchSecondaryWeapon();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.SwitchToThirdWeapon))
                {
                    NotifySwitchThridWeapon();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.Interact))
                {
                    NotifyInteract();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.Reload))
                {
                    NotifyReload();
                }
                if (keyboard.GetKeyDown(ActionKey.Instance.ChangeViewMode))
                {
                    NotifyChangeViewMode();
                }
            }

            private void HandleDirectionInput()
            {
                List<KeyCode> keys = new List<KeyCode>();

                if (keyboard.GetKey(ActionKey.Instance.MoveFoward))
                {
                    keys.Add(ActionKey.Instance.MoveFoward);
                }
                if (keyboard.GetKey(ActionKey.Instance.MoveLeft))
                {
                    keys.Add(ActionKey.Instance.MoveLeft);
                }
                if (keyboard.GetKey(ActionKey.Instance.MoveBackward))
                {
                    keys.Add(ActionKey.Instance.MoveBackward);
                }
                if (keyboard.GetKey(ActionKey.Instance.MoveRight))
                {
                    keys.Add(ActionKey.Instance.MoveRight);
                }

                if(keys.Count > 0)
                    NotifyMove(keys);
            }
        }
    }
}