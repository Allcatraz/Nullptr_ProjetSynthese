using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/HighScoresMenuController")]
    public class HighScoresMenuController : GameScript, IMenuController
    {
        private Selectable okButton;
        private HighScoreRepository highScoreRepository;
        private HighScoreListView highScoreListView;
        private ActivityStack activityStack;

        private void InjectHighScoresController([Named(R.S.GameObject.OkButton)][EntityScope] Selectable okButton,
                                               [ApplicationScope] HighScoreRepository highScoreRepository,
                                               [EntityScope] HighScoreListView highScoreListView,
                                               [ApplicationScope] ActivityStack activityStack)
        {
            this.okButton = okButton;
            this.highScoreRepository = highScoreRepository;
            this.highScoreListView = highScoreListView;
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectHighScoresController");
        }

        public void OnCreate(params object[] parameters)
        {
            highScoreListView.SetHighScores(highScoreRepository.GetAllHighScores());
        }

        public void OnResume()
        {
            okButton.Select();
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
        public void QuitHighScores()
        {
            activityStack.StopCurrentMenu();
        }
    }
}