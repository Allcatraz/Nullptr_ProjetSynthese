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

        private void InjectMouseInputDevice([ApplicationScope] Mouse mouse)
        {
            this.mouse = mouse;
        }

        private void Awake()
        {
            InjectDependencies("InjectMouseInputDevice");

            mouseInputDevice = new MouseInputDevice(mouse);
        }

        private void Update()
        {
            mouseInputDevice.Update();
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
