using Harmony;
using Harmony.Injection;
using Harmony.Unity;
using Harmony.Util;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Control/MainMenuController")]
    public class MainMenuController : GameScript, IMenuController
    {
        [SerializeField]
        private UnityActivity gameActivity;

        [SerializeField]
        private UnityMenu highScoresMenu;

        private ISelectable playButton;
        private IActivityStack activityStack;
        private IMenuStack menuStack;

        public void InjectMainMenuController(UnityActivity gameActivity,
                                             UnityMenu highScoresMenu,
                                             [Named(R.S.GameObject.PlayButton)] [EntityScope] ISelectable playButton,
                                             [ApplicationScope] IActivityStack activityStack,
                                             [ApplicationScope] IMenuStack menuStack)
        {
            this.gameActivity = gameActivity;
            this.highScoresMenu = highScoresMenu;
            this.playButton = playButton;
            this.activityStack = activityStack;
            this.menuStack = menuStack;
        }

        public void Awake()
        {
            InjectDependencies("InjectMainMenuController",
                               gameActivity,
                               highScoresMenu);
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
            menuStack.StartMenu(highScoresMenu);
        }

        [CalledOutsideOfCode]
        public void QuitGame()
        {
            activityStack.StopCurrentActivity();
        }
    }
}