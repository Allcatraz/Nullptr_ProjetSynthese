using System;
using Harmony;
using Harmony.Injection;
using Harmony.Unity;
using Harmony.Util;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Control/GameOverMenuController")]
    public class GameOverMenuController : GameScript, IMenuController
    {
        [SerializeField]
        private UnityMenu newHisghScoreMenu;

        private ISelectable retryButton;
        private HighScoreRepository highScoreRepository;
        private IActivityStack activityStack;
        private IMenuStack menuStack;

        public void InjectGameOverController(UnityMenu newHisghScoreMenu,
                                             [Named(R.S.GameObject.RetryButton)] [EntityScope] ISelectable retryButton,
                                             [ApplicationScope] HighScoreRepository highScoreRepository,
                                             [ApplicationScope] IActivityStack activityStack,
                                             [ApplicationScope] IMenuStack menuStack)
        {
            this.newHisghScoreMenu = newHisghScoreMenu;
            this.retryButton = retryButton;
            this.highScoreRepository = highScoreRepository;
            this.activityStack = activityStack;
            this.menuStack = menuStack;
        }

        public void Awake()
        {
            InjectDependencies("InjectGameOverController",
                               newHisghScoreMenu);
        }

        public void OnCreate(params object[] parameters)
        {
            GameEvent gameEvent = parameters[0] as GameEvent;
            if (gameEvent == null)
            {
                throw new ArgumentException("GameOverMenuController expects a GameEvent as paramater for the OnCreate method.");
            }
            if (IsNewHighScore(gameEvent.Score))
            {
                ShowNewHighScore(gameEvent.Score);
            }
        }

        public void OnResume()
        {
            retryButton.Select();
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
        public void RetryGame()
        {
            activityStack.RestartCurrentActivity();
        }

        [CalledOutsideOfCode]
        public void QuitGame()
        {
            activityStack.StopCurrentActivity();
        }

        private bool IsNewHighScore(Score score)
        {
            HighScore lowestHighScore = highScoreRepository.GetLowestHighScore();
            return lowestHighScore == null ||
                   !highScoreRepository.IsLeaderboardFull() ||
                   score.ScorePoints > lowestHighScore.ScorePoints;
        }

        private void ShowNewHighScore(Score score)
        {
            menuStack.StartMenu(newHisghScoreMenu, score);
        }
    }
}