using UnityEngine;

namespace ProjetSynthese
{
    //Menu actions
    public delegate void UpEventHandler();
    public delegate void DownEventHandler();
    public delegate void ConfirmEventHandler();

    //Game actions
    public delegate void TogglePauseEventHandler();
    public delegate void ToggleInventoryEventHandler();
    public delegate void ToggleMapEventHandler();

    public delegate void MoveTowardEventHandler(Vector3 direction);
    public delegate void SwitchSprintOnEventHandler();
    public delegate void SwitchSprintOffEventHandler();

    public delegate void SwitchPrimaryWeaponEventHandler();
    public delegate void SwitchSecondaryWeaponEventHandler();
    public delegate void SwitchThirdWeaponEventHandler();

    public delegate void FireEventHandler();
    
    public delegate void PickupEventHandler();

    public delegate void ReloadEventHandler();

    public interface IInputDevice
    {
        //Menu Action
        event UpEventHandler OnUp;
        event DownEventHandler OnDown;
        event ConfirmEventHandler OnConfirm;

        //Game Action
        event TogglePauseEventHandler OnTogglePause;
        event ToggleInventoryEventHandler OnToggleInventory;
        event ToggleMapEventHandler OnToggleMap;

        event MoveTowardEventHandler OnMoveToward;
        event SwitchSprintOnEventHandler OnSwitchSprintOn;
        event SwitchSprintOffEventHandler OnSwitchSprintOff;

        event SwitchPrimaryWeaponEventHandler OnSwitchPrimaryWeapon;
        event SwitchSecondaryWeaponEventHandler OnSwitchSecondaryWeapon;
        event SwitchThirdWeaponEventHandler OnSwitchThridWeapon;

        event FireEventHandler OnFire;
       
        event PickupEventHandler OnPickup;

        event ReloadEventHandler OnReload;

        IInputDevice this[int deviceIndex] { get; }
    }
}