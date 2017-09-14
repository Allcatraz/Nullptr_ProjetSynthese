using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Input/GlobalMenuController")]
    public class GlobalMenuController : GameScript
    {
        private PlayerInputSensor playerInputSensor;
        private IMenuState menuState;

        public void InjectGlobalMenuController([ApplicationScope] PlayerInputSensor playerInputSensor,
                                               [ApplicationScope] IMenuState menuState)
        {
            this.playerInputSensor = playerInputSensor;
            this.menuState = menuState;
        }

        public void Awake()
        {
            InjectDependencies("InjectGlobalMenuController");
        }

        public void OnEnable()
        {
            playerInputSensor.Players.OnUp += OnUp;
            playerInputSensor.Players.OnDown += OnDown;
            playerInputSensor.Players.OnConfirm += OnConfirm;
        }

        public void OnDisable()
        {
            playerInputSensor.Players.OnUp -= OnUp;
            playerInputSensor.Players.OnDown -= OnDown;
            playerInputSensor.Players.OnConfirm -= OnConfirm;
        }

        private void OnUp()
        {
            ISelectable currentSelectable = menuState.CurrentSelected;
            if (currentSelectable != null)
            {
                currentSelectable.SelectPrevious();
            }
        }

        private void OnDown()
        {
            ISelectable currentSelectable = menuState.CurrentSelected;
            if (currentSelectable != null)
            {
                currentSelectable.SelectNext();
            }
        }

        private void OnConfirm()
        {
            ISelectable currentSelectable = menuState.CurrentSelected;
            if (currentSelectable != null)
            {
                currentSelectable.Click();
            }
        }
    }
}