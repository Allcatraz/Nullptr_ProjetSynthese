using System;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/GameOverMenuController")]
    public class GameOverMenuController : GameScript, IMenuController
    {
        [SerializeField]
        private Menu newHisghScoreMenu;

        private Selectable retryButton;
        private HighScoreRepository highScoreRepository;
        private ActivityStack activityStack;

        private void InjectGameOverController([Named(R.S.GameObject.RetryButton)] [EntityScope] Selectable retryButton,
                                             [ApplicationScope] HighScoreRepository highScoreRepository,
                                             [ApplicationScope] ActivityStack activityStack)
        {
            this.retryButton = retryButton;
            this.highScoreRepository = highScoreRepository;
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectGameOverController");
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
            return lowestHighScore == null || !highScoreRepository.IsLeaderboardFull() || score.ScorePoints > lowestHighScore.ScorePoints;
        }

        private void ShowNewHighScore(Score score)
        {
            activityStack.StartMenu(newHisghScoreMenu, score);
        }
    }
}