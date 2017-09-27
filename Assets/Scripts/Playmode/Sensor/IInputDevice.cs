using UnityEngine;

namespace ProjetSynthese
{
    //Menu actions
    public delegate void UpEventHandler();

    public delegate void DownEventHandler();

    public delegate void ConfirmEventHandler();

    //Game actions
    public delegate void TogglePauseEventHandler();

    public delegate void FireEventHandler();

    public delegate void MoveTowardHandler(Vector3 direction);

    public interface IInputDevice
    {
        event UpEventHandler OnUp;
        event DownEventHandler OnDown;
        event ConfirmEventHandler OnConfirm;
        event TogglePauseEventHandler OnTogglePause;
        event FireEventHandler OnFire;
        event MoveTowardHandler OnMove;

        IInputDevice this[int deviceIndex] { get; }
    }
}