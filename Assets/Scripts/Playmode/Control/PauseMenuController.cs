using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Control/PauseMenuController")]
    public class PauseMenuController : GameScript, IMenuController
    {
        private Selectable resumeButton;
        private ActivityStack activityStack;

        private void InjectPauseController([Named(R.S.GameObject.ResumeButton)] [EntityScope] Selectable resumeButton,
                                          [ApplicationScope] ActivityStack activityStack)
        {
            this.resumeButton = resumeButton;
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectPauseController");
        }

        public void OnCreate(params object[] parameters)
        {
            TimeExtensions.Pause();
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
            TimeExtensions.Resume();
        }

        [CalledOutsideOfCode]
        public void ResumeGame()
        {
            activityStack.StopCurrentMenu();
        }

        [CalledOutsideOfCode]
        public void QuitGame()
        {
            activityStack.StopCurrentActivity();
        }
    }
}