namespace ProjetSynthese
{
    public abstract class InputSensor : GameScript
    {
        protected abstract class InputDevice : IInputDevice
        {
            public event UpEventHandler OnUp;
            public event DownEventHandler OnDown;
            public event ConfirmEventHandler OnConfirm;
            public event TogglePauseEventHandler OnTogglePause;
            public event FireEventHandler OnFire;
            public event FowardEventHandler OnFoward;
            public event BackwardEventHandler OnBackward;
            public event RotateLeftEventHandler OnRotateLeft;
            public event RotateRightEventHandler OnRotateRight;

            public abstract IInputDevice this[int deviceIndex] { get; }

            protected virtual void NotifyUp()
            {
                if (OnUp != null) OnUp();
            }

            protected virtual void NotifyDown()
            {
                if (OnDown != null) OnDown();
            }

            protected virtual void NotifyConfirm()
            {
                if (OnConfirm != null) OnConfirm();
            }

            protected virtual void NotifyTogglePause()
            {
                if (OnTogglePause != null) OnTogglePause();
            }

            protected virtual void NotifyFire()
            {
                if (OnFire != null) OnFire();
            }

            protected virtual void NotifyFoward()
            {
                if (OnFoward != null) OnFoward();
            }

            protected virtual void NotifyBackward()
            {
                if (OnBackward != null) OnBackward();
            }

            protected virtual void NotifyRotateLeft()
            {
                if (OnRotateLeft != null) OnRotateLeft();
            }

            protected virtual void NotifyRotateRight()
            {
                if (OnRotateRight != null) OnRotateRight();
            }
        }

        protected abstract class TriggerOncePerFrameInputDevice : InputDevice
        {
            private bool upTriggerd;
            private bool downTriggerd;
            private bool confirmedTriggerd;
            private bool togglePauseTriggerd;
            private bool fireTriggerd;
            private bool fowardTriggerd;
            private bool backwardTriggerd;
            private bool rotateLeftTriggerd;
            private bool rotateRightTriggerd;

            public abstract override IInputDevice this[int deviceIndex] { get; }

            public virtual void Reset()
            {
                upTriggerd = false;
                downTriggerd = false;
                confirmedTriggerd = false;
                togglePauseTriggerd = false;
                fireTriggerd = false;
                fowardTriggerd = false;
                backwardTriggerd = false;
                rotateLeftTriggerd = false;
                rotateRightTriggerd = false;
            }

            protected override void NotifyUp()
            {
                if (!upTriggerd)
                {
                    base.NotifyUp();
                    upTriggerd = true;
                }
            }

            protected override void NotifyDown()
            {
                if (!downTriggerd)
                {
                    base.NotifyDown();
                    downTriggerd = true;
                }
            }

            protected override void NotifyConfirm()
            {
                if (!confirmedTriggerd)
                {
                    base.NotifyConfirm();
                    confirmedTriggerd = true;
                }
            }

            protected override void NotifyTogglePause()
            {
                if (!togglePauseTriggerd)
                {
                    base.NotifyTogglePause();
                    togglePauseTriggerd = true;
                }
            }

            protected override void NotifyFire()
            {
                if (!fireTriggerd)
                {
                    base.NotifyFire();
                    fireTriggerd = true;
                }
            }

            protected override void NotifyFoward()
            {
                if (!fowardTriggerd)
                {
                    base.NotifyFoward();
                    fowardTriggerd = true;
                }
            }

            protected override void NotifyBackward()
            {
                if (!backwardTriggerd)
                {
                    base.NotifyBackward();
                    backwardTriggerd = true;
                }
            }

            protected override void NotifyRotateLeft()
            {
                if (!rotateLeftTriggerd)
                {
                    base.NotifyRotateLeft();
                    rotateLeftTriggerd = true;
                }
            }

            protected override void NotifyRotateRight()
            {
                if (!rotateRightTriggerd)
                {
                    base.NotifyRotateRight();
                    rotateRightTriggerd = true;
                }
            }
        }
    }
}