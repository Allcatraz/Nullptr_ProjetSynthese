using System;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/NewHighScoreMenuController")]
    public class NewHighScoreMenuController : GameScript, IMenuController
    {
        private InputField nameInput;
        private Selectable okButton;
        private ActivityStack activityStack;
        private HighScoreRepository highScoreRepository;

        private Score score;

        private void InjectNewHighScoreController([Named(R.S.GameObject.NameInput)] [EntityScope] InputField nameInput,
                                                 [Named(R.S.GameObject.OkButton)] [EntityScope] Selectable okButton,
                                                 [ApplicationScope] ActivityStack activityStack,
                                                 [ApplicationScope] HighScoreRepository highScoreRepository)
        {
            this.nameInput = nameInput;
            this.okButton = okButton;
            this.activityStack = activityStack;
            this.highScoreRepository = highScoreRepository;
        }

        private void Awake()
        {
            InjectDependencies("InjectNewHighScoreController");
        }

        public void OnCreate(params object[] parameters)
        {
            score = parameters[0] as Score;
            if (score == null)
            {
                throw new ArgumentException("NewHighScoreMenuController expects a Score as paramater for the OnCreate method.");
            }
        }

        public void OnResume()
        {
            nameInput.Select();
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
        public void EndScoreEdit()
        {
            okButton.Select();
        }

        [CalledOutsideOfCode]
        public void SaveHighScore()
        {
            highScoreRepository.AddScore(new HighScore
            {
                Name = nameInput.text,
                ScorePoints = score.ScorePoints
            });

            activityStack.StopCurrentMenu();
        }
    }
}