using Harmony;
using Harmony.Injection;
using Harmony.Util;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Control/HighScoresMenuController")]
    public class HighScoresMenuController : GameScript, IMenuController
    {
        private ISelectable okButton;
        private HighScoreRepository highScoreRepository;
        private HighScoreListView highScoreListView;
        private IMenuStack menuStack;

        public void InjectHighScoresController([Named(R.S.GameObject.OkButton)][EntityScope] ISelectable okButton,
                                               [ApplicationScope] HighScoreRepository highScoreRepository,
                                               [EntityScope] HighScoreListView highScoreListView,
                                               [ApplicationScope] IMenuStack menuStack)
        {
            this.okButton = okButton;
            this.highScoreRepository = highScoreRepository;
            this.highScoreListView = highScoreListView;
            this.menuStack = menuStack;
        }

        public void Awake()
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
            menuStack.StopCurrentMenu();
        }
    }
}