using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/MainMenuController")]
    public class MainMenuController : GameScript, IMenuController
    {
        [SerializeField]
        private Activity gameActivity;

        [SerializeField]
        private Menu highScoresMenu;

        private Selectable playButton;
        private ActivityStack activityStack;

        private void InjectMainMenuController([Named(R.S.GameObject.PlayButton)] [EntityScope] Selectable playButton,
                                             [ApplicationScope] ActivityStack activityStack)
        {
            this.playButton = playButton;
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectMainMenuController");
        }

        public void OnCreate(params object[] parameters)
        {
            //Nothing to do
        }

        public void OnResume()
        {
            playButton.Select();
        }

        public void OnPause()
        {
            //Nothing to do
        }

        public void OnStop()
        {
            //Nothing to do
        }

        [CalledOutsideOfCode]
        public void StartGame()
        {
            activityStack.StartActivity(gameActivity);
        }

        [CalledOutsideOfCode]
        public void ShowHighScores()
        {
            activityStack.StartMenu(highScoresMenu);
        }

        [CalledOutsideOfCode]
        public void QuitGame()
        {
            activityStack.StopCurrentActivity();
        }
    }
}