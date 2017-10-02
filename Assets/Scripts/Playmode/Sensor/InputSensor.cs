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

            public event ToggleInventoryEventHandler OnToggleInventory;
            public event ToggleMapEventHandler OnToggleMap;
            public event TogglePauseEventHandler OnTogglePause;

            public event MoveTowardEventHandler OnMoveToward;
            public event SwitchSprintOnEventHandler OnSwitchSprintOn;
            public event SwitchSprintOffEventHandler OnSwitchSprintOff;

            public event SwitchPrimaryWeaponEventHandler OnSwitchPrimaryWeapon;
            public event SwitchSecondaryWeaponEventHandler OnSwitchSecondaryWeapon;
            public event SwitchThirdWeaponEventHandler OnSwitchThridWeapon;

            public event FireEventHandler OnFire;

            public event PickupEventHandler OnPickup;

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

            protected virtual void NotifyToggleInventory()
            {
                if (OnToggleInventory != null) OnToggleInventory();
            }

            protected virtual void NotifyToggleMap()
            {
                if (OnToggleMap != null) OnToggleMap();
            }

            protected virtual void NotifyTogglePause()
            {
                if (OnTogglePause != null) OnTogglePause();
            }

            protected virtual void NotifyMove(Vector3 direction)
            {
                if (OnMoveToward != null) OnMoveToward(direction);
            }

            protected virtual void NotifySwitchSprintOn()
            {
                if (OnSwitchSprintOn != null) OnSwitchSprintOn();
            }

            protected virtual void NotifySwitchSprintOff()
            {
                if (OnSwitchSprintOff != null) OnSwitchSprintOff();
            }

            protected virtual void NotifySwitchPrimaryWeapon()
            {
                if (OnSwitchPrimaryWeapon != null) OnSwitchPrimaryWeapon();
            }

            protected virtual void NotifySwitchSecondaryWeapon()
            {
                if (OnSwitchSecondaryWeapon != null) OnSwitchSecondaryWeapon();
            }

            protected virtual void NotifySwitchThridWeapon()
            {
                if (OnSwitchThridWeapon != null) OnSwitchThridWeapon();
            }

            protected virtual void NotifyFire()
            {
                if (OnFire != null) OnFire();
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
            private bool mapTriggerd;
            private bool togglePauseTriggerd;

            private bool moveTriggerd;
            private bool switchSprintOnTriggerd;
            private bool switchSrintOffTriggerd;

            private bool switchPrimaryWeaponTriggerd;
            private bool switchSecondaryWeaponTriggerd;
            private bool switchThirdWeaponTriggerd;

            private bool fireTriggerd;

            private bool pickupTriggerd;

            public abstract override IInputDevice this[int deviceIndex] { get; }

            public virtual void Reset()
            {
                upTriggerd = false;
                downTriggerd = false;
                confirmedTriggerd = false;

                inventoryTriggerd = false;
                mapTriggerd = false;
                togglePauseTriggerd = false;

                moveTriggerd = false;
                switchSprintOnTriggerd = false;
                switchSrintOffTriggerd = false;

                switchPrimaryWeaponTriggerd = false;
                switchSecondaryWeaponTriggerd = false;
                switchThirdWeaponTriggerd = false;

                fireTriggerd = false;

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

            protected override void NotifyToggleInventory()
            {
                if (!inventoryTriggerd)
                {
                    base.NotifyToggleInventory();
                    inventoryTriggerd = true;
                }
            }

            protected override void NotifyToggleMap()
            {
                if (!mapTriggerd)
                {
                    base.NotifyToggleMap();
                    mapTriggerd = true;
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

            protected override void NotifyMove(Vector3 direction)
            {
                if (!moveTriggerd)
                {
                    base.NotifyMove(direction);
                    moveTriggerd = true;
                }
            }

            protected override void NotifySwitchSprintOn()
            {
                if (!switchSprintOnTriggerd)
                {
                    base.NotifySwitchSprintOn();
                    switchSprintOnTriggerd = true;
                }
            }

            protected override void NotifySwitchSprintOff()
            {
                if (!switchSrintOffTriggerd)
                {
                    base.NotifySwitchSprintOff();
                    switchSrintOffTriggerd = true;
                }
            }

            protected override void NotifySwitchPrimaryWeapon()
            {
                if (!switchPrimaryWeaponTriggerd)
                {
                    base.NotifySwitchPrimaryWeapon();
                    switchPrimaryWeaponTriggerd = true;
                }
            }

            protected override void NotifySwitchSecondaryWeapon()
            {
                if (!switchSecondaryWeaponTriggerd)
                {
                    base.NotifySwitchSecondaryWeapon();
                    switchSecondaryWeaponTriggerd = true;
                }              
            }

            protected override void NotifySwitchThridWeapon()
            {
                if (!switchThirdWeaponTriggerd)
                {
                    base.NotifySwitchThridWeapon();
                    switchThirdWeaponTriggerd = true;
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

            protected override void NotifyPickup()
            {
                if (!pickupTriggerd)
                {
                    base.NotifyPickup();
                    pickupTriggerd = true;
                }
            }
        }
    }
}