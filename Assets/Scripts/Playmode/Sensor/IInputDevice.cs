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

    public delegate void SwitchSprintOnHandler();
    public delegate void SwitchSprintOffHandler();

    public delegate void FireEventHandler();
    public delegate void MoveTowardHandler(Vector3 direction);
    public delegate void PickupHandler();

    public interface IInputDevice
    {
        event UpEventHandler OnUp;
        event DownEventHandler OnDown;
        event ConfirmEventHandler OnConfirm;

        event ToggleInventoryEventHandler OnInventoryAction;
        event TogglePauseEventHandler OnTogglePause;

        event SwitchSprintOnHandler OnSwitchSprintOn;
        event SwitchSprintOffHandler OnSwitchSprintOff;

        event FireEventHandler OnFire;
        event MoveTowardHandler OnMove;
        event PickupHandler OnPickup;

        IInputDevice this[int deviceIndex] { get; }
    }
}