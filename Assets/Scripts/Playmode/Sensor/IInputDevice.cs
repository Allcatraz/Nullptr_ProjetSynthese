using UnityEngine;

namespace ProjetSynthese
{
    //Menu actions
    public delegate void UpEventHandler();

    public delegate void DownEventHandler();

    public delegate void ConfirmEventHandler();

    //Game actions
    public delegate void TogglePauseEventHandler();

    public delegate void InventoryEventHandler();

    public delegate void FireEventHandler();

    public delegate void MoveTowardHandler(Vector3 direction);

    public delegate void PickupHandler();

    public interface IInputDevice
    {
        event UpEventHandler OnUp;
        event DownEventHandler OnDown;
        event ConfirmEventHandler OnConfirm;

        event InventoryEventHandler OnInventoryAction;
        event TogglePauseEventHandler OnTogglePause;
        event FireEventHandler OnFire;
        event MoveTowardHandler OnMove;
        event PickupHandler OnPickup;

        IInputDevice this[int deviceIndex] { get; }
    }
}