using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/ShowPauseMenuOnPlayerPause")]
    public class ShowPauseMenuOnPlayerPause : GameScript
    {
        [SerializeField]
        private Menu pauseMenu;

        private PlayerInputSensor playerInputSensor;
        private ActivityStack activityStack;

        private void InjectShowPauseMenuOnPlayerPause([ApplicationScope] PlayerInputSensor playerInputSensor,
                                                     [ApplicationScope] ActivityStack activityStack)
        {
            this.playerInputSensor = playerInputSensor;
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectShowPauseMenuOnPlayerPause");
        }

        private void OnEnable()
        {
            playerInputSensor.Players.OnTogglePause += OnTogglePause;
        }

        private void OnDisable()
        {
            playerInputSensor.Players.OnTogglePause -= OnTogglePause;
        }

        private void OnTogglePause()
        {
            if (TimeExtensions.IsRunning())
            {
                activityStack.StartMenu(pauseMenu);
            }
        }
    }
}