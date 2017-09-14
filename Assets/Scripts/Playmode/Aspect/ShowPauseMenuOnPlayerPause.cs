using UnityEngine;
using Harmony;
using Harmony.Injection;
using Harmony.Unity;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Aspect/ShowPauseMenuOnPlayerPause")]
    public class ShowPauseMenuOnPlayerPause : GameScript
    {
        [SerializeField]
        private UnityMenu pauseMenu;

        private PlayerInputSensor playerInputSensor;
        private ITime time;
        private IMenuStack menuStack;

        public void InjectShowPauseMenuOnPlayerPause(UnityMenu pauseMenu,
                                                     [ApplicationScope] PlayerInputSensor playerInputSensor,
                                                     [ApplicationScope] ITime time,
                                                     [ApplicationScope] IMenuStack menuStack)
        {
            this.pauseMenu = pauseMenu;
            this.playerInputSensor = playerInputSensor;
            this.time = time;
            this.menuStack = menuStack;
        }

        public void Awake()
        {
            InjectDependencies("InjectShowPauseMenuOnPlayerPause",
                               pauseMenu);
        }

        public void OnEnable()
        {
            playerInputSensor.Players.OnTogglePause += OnTogglePause;
        }

        public void OnDisable()
        {
            playerInputSensor.Players.OnTogglePause -= OnTogglePause;
        }

        private void OnTogglePause()
        {
            if (!time.IsPaused())
            {
                menuStack.StartMenu(pauseMenu);
            }
        }
    }
}