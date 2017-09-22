namespace ProjetSynthese
{
    //Menu actions
    public delegate void UpEventHandler();

    public delegate void DownEventHandler();

    public delegate void ConfirmEventHandler();

    //Game actions
    public delegate void TogglePauseEventHandler();

    public delegate void FireEventHandler();

    public delegate void FowardEventHandler();

    public delegate void BackwardEventHandler();

    public delegate void RotateLeftEventHandler();

    public delegate void RotateRightEventHandler();

    public interface IInputDevice
    {
        event UpEventHandler OnUp;
        event DownEventHandler OnDown;
        event ConfirmEventHandler OnConfirm;
        event TogglePauseEventHandler OnTogglePause;
        event FireEventHandler OnFire;
        event FowardEventHandler OnFoward;
        event BackwardEventHandler OnBackward;
        event RotateLeftEventHandler OnRotateLeft;
        event RotateRightEventHandler OnRotateRight;

        IInputDevice this[int deviceIndex] { get; }
    }
}