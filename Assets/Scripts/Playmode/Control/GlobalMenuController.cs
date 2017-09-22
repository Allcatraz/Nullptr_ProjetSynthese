using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/GlobalMenuController")]
    public class GlobalMenuController : GameScript
    {
        private PlayerInputSensor playerInputSensor;

        private void InjectGlobalMenuController([ApplicationScope] PlayerInputSensor playerInputSensor)
        {
            this.playerInputSensor = playerInputSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectGlobalMenuController");

            playerInputSensor.Players.OnUp += OnUp;
            playerInputSensor.Players.OnDown += OnDown;
            playerInputSensor.Players.OnConfirm += OnConfirm;
        }

        private void OnDestroy()
        {
            playerInputSensor.Players.OnUp -= OnUp;
            playerInputSensor.Players.OnDown -= OnDown;
            playerInputSensor.Players.OnConfirm -= OnConfirm;
        }

        private void OnUp()
        {
            Selectable currentSelectable = SelectableExtensions.GetCurrentlySelected();
            if (currentSelectable != null)
            {
                currentSelectable.SelectPrevious();
            }
        }

        private void OnDown()
        {
            Selectable currentSelectable = SelectableExtensions.GetCurrentlySelected();
            if (currentSelectable != null)
            {
                currentSelectable.SelectNext();
            }
        }

        private void OnConfirm()
        {
            Selectable currentSelectable = SelectableExtensions.GetCurrentlySelected();
            if (currentSelectable != null)
            {
                currentSelectable.Click();
            }
        }
    }
}