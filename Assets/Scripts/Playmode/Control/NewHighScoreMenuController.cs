using System;
using Harmony;
using Harmony.Injection;
using Harmony.Util;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Control/NewHighScoreMenuController")]
    public class NewHighScoreMenuController : GameScript, IMenuController
    {
        private ITextInput nameInput;
        private ISelectable okButton;
        private IMenuStack menuStack;
        private HighScoreRepository highScoreRepository;

        private Score score;

        public void InjectNewHighScoreController([Named(R.S.GameObject.NameInput)] [EntityScope] ITextInput nameInput,
                                                 [Named(R.S.GameObject.OkButton)] [EntityScope] ISelectable okButton,
                                                 [ApplicationScope] IMenuStack menuStack,
                                                 [ApplicationScope] HighScoreRepository highScoreRepository)
        {
            this.nameInput = nameInput;
            this.okButton = okButton;
            this.menuStack = menuStack;
            this.highScoreRepository = highScoreRepository;
        }

        public void Awake()
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
                Name = nameInput.Text,
                ScorePoints = score.ScorePoints
            });

            menuStack.StopCurrentMenu();
        }
    }
}