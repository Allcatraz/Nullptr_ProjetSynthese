using System.Collections.Generic;
using UnityEngine;
using Harmony;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/View/HighScoreListView")]
    public class HighScoreListView : GameScript
    {
        private GameObject contentView;
        private HighScoreViewSpawner highScoreViewSpawner;

        public void InjectHighScoresView([Named(R.S.GameObject.Content)] [ChildScope] GameObject contentView,
                                         [SceneScope] HighScoreViewSpawner highScoreViewSpawner)
        {
            this.contentView = contentView;
            this.highScoreViewSpawner = highScoreViewSpawner;
        }

        public void Awake()
        {
            InjectDependencies("InjectHighScoresView");
        }

        public virtual void SetHighScores(IList<HighScore> highScores)
        {
            for (int i = 0; i < contentView.transform.childCount; i++)
            {
                Destroy(contentView.transform.GetChild(i).gameObject);
            }

            foreach (HighScore highScore in highScores)
            {
                highScoreViewSpawner.Spawn(contentView, highScore);
            }
        }
    }
}