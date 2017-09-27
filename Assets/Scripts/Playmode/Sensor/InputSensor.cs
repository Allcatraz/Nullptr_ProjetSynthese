using UnityEngine;

namespace ProjetSynthese
{
    public abstract class InputSensor : GameScript
    {
        protected abstract class InputDevice : IInputDevice
        {
            public event UpEventHandler OnUp;
            public event DownEventHandler OnDown;
            public event ConfirmEventHandler OnConfirm;

            public event InventoryEventHandler OnInventoryAction;
            public event TogglePauseEventHandler OnTogglePause;
            public event FireEventHandler OnFire;
            public event MoveTowardHandler OnMove;
            public event PickupHandler OnPickup;

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

            protected virtual void NotifyMove(Vector3 direction)
            {
                if (OnMove != null) OnMove(direction);
            }

            protected virtual void NotifyInventoryAction()
            {
                if (OnInventoryAction != null) OnInventoryAction();
            }

            protected virtual void NotifyPickup()
            {
                if (OnPickup != null) OnPickup();
            }
        }

        protected abstract class TriggerOncePerFrameInputDevice : InputDevice
        {
            private bool upTriggerd;
            private bool downTriggerd;
            private bool confirmedTriggerd;

            private bool inventoryTriggerd;
            private bool togglePauseTriggerd;
            private bool fireTriggerd;
            private bool moveTriggerd;
            private bool pickupTriggerd;

            public abstract override IInputDevice this[int deviceIndex] { get; }

            public virtual void Reset()
            {
                upTriggerd = false;
                downTriggerd = false;
                confirmedTriggerd = false;

                inventoryTriggerd = false;
                togglePauseTriggerd = false;
                fireTriggerd = false;
                moveTriggerd = false;
                pickupTriggerd = false;
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

            protected override void NotifyMove(Vector3 direction)
            {
                if (!moveTriggerd)
                {
                    base.NotifyMove(direction);
                    moveTriggerd = true;
                }
            }

            protected override void NotifyInventoryAction()
            {
                if (!inventoryTriggerd)
                {
                    base.NotifyInventoryAction();
                    inventoryTriggerd = true;
                }
            }

            protected override void NotifyPickup()
            {
                if (!pickupTriggerd)
                {
                    base.NotifyPickup();
                    pickupTriggerd = false;
                }
            }
        }
    }
}