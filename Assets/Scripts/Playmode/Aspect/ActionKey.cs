using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ProjetSynthese
{
    public class ActionKey : GameScript
    {
        public static ActionKey Instance;

        public KeyCode MoveFoward { get; set; }
        public KeyCode MoveBackward { get; set; }
        public KeyCode MoveLeft { get; set; }
        public KeyCode MoveRight { get; set; }

        public KeyCode ToggleInventory { get; set; }
        public KeyCode ToggleMap { get; set; }
        public KeyCode TogglePause { get; set; }

        public KeyCode ToggleSprint { get; set; }

        public KeyCode SwitchToPrimaryWeapon { get; set; }
        public KeyCode SwitchToSecondaryWeapon { get; set; }
        public KeyCode SwitchToThirdWeapon { get; set; }

        public KeyCode Interact { get; set; }
        public KeyCode Reload { get; set; }
        public KeyCode ChangeViewMode { get; set; }

        public MouseButton Fire { get; set; }

        public KeyCode ChangeWeaponSlot { get; set; }
        public KeyCode DropItemTrigger { get; set; }

        private void Start()
        {
            Instance = this;

            MoveFoward = KeyCode.W;
            MoveBackward = KeyCode.S;
            MoveLeft = KeyCode.A;
            MoveRight = KeyCode.D;

            ToggleInventory = KeyCode.Tab;
            ToggleMap = KeyCode.M;
            TogglePause = KeyCode.Escape;

            ToggleSprint = KeyCode.LeftShift;

            SwitchToPrimaryWeapon = KeyCode.Alpha1;
            SwitchToSecondaryWeapon = KeyCode.Alpha2;
            SwitchToThirdWeapon = KeyCode.Alpha3;

            Interact = KeyCode.F;
            Reload = KeyCode.R;
            ChangeViewMode = KeyCode.F9;

            Fire = MouseButton.LeftMouse;

            ChangeWeaponSlot = KeyCode.LeftControl;
            DropItemTrigger = KeyCode.LeftAlt;
        }
    }
}
