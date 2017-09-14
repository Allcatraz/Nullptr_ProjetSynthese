using Harmony;
using Harmony.Injection;
using Harmony.Util;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Control/PauseMenuController")]
    public class PauseMenuController : GameScript, IMenuController
    {
        private ISelectable resumeButton;
        private ITime time;
        private IActivityStack activityStack;
        private IMenuStack menuStack;

        public void InjectPauseController([Named(R.S.GameObject.ResumeButton)] [EntityScope] ISelectable resumeButton,
                                          [ApplicationScope] ITime time,
                                          [ApplicationScope] IActivityStack activityStack,
                                          [ApplicationScope] IMenuStack menuStack)
        {
            this.resumeButton = resumeButton;
            this.time = time;
            this.activityStack = activityStack;
            this.menuStack = menuStack;
        }

        public void Awake()
        {
            InjectDependencies("InjectPauseController");
        }

        public void OnCreate(params object[] parameters)
        {
            time.Pause();
        }

        public void OnResume()
        {
            resumeButton.Select();
        }

        public void OnPause()
        {
            //Nothing to do
        }

        public void OnStop()
        {
            time.Resume();
        }

        [CalledOutsideOfCode]
        public void ResumeGame()
        {
            menuStack.StopCurrentMenu();
        }

        [CalledOutsideOfCode]
        public void QuitGame()
        {
            activityStack.StopCurrentActivity();
        }
    }
}