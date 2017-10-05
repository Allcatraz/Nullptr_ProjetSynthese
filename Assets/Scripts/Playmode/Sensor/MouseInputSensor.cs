using UnityEngine;
using Harmony;
using UnityEngine.Experimental.UIElements;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Sensor/MouseInputSensor")]
    public class MouseInputSensor : InputSensor
    {
        private Mouse mouse;

        private MouseInputDevice mouseInputDevice;

        public IInputDevice Mouses
        {
            get { return mouseInputDevice; }
        }

        private void InjectMouseInputSensor([GameObjectScope] Mouse mouse)
        {
            this.mouse = mouse;
        }

        private void Awake()
        {
            InjectDependencies("InjectMouseInputSensor");
            mouseInputDevice = new MouseInputDevice(mouse);
        }

        private void Update()
        {
            mouseInputDevice.Update();
        }

        public Vector3 GetPosition()
        {
            return mouse.GetMousePosition();
        }

        private class MouseInputDevice : InputDevice
        {
            private readonly Mouse mouse;

            public MouseInputDevice(Mouse mouse)
            {
                this.mouse = mouse;
            }

            public void Update()
            {
                HandleActionInput();
            }

            public override IInputDevice this[int deviceIndex]
            {
                get { return this; }
            }

            private void HandleActionInput()
            {
                if (mouse.GetMouseButton(MouseButton.LeftMouse))
                {
                    NotifyFire();
                }
            }
        }
    }
}
