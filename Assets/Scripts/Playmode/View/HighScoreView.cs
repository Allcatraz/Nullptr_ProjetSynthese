using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/View/HighScoreView")]
    public class HighScoreView : GameScript
    {
        [SerializeField]
        private string highScoreFormat  = "{0} : {1}";

        private Text textView;

        private void InjectHighScoreView([GameObjectScope] Text textView)
        {
            this.textView = textView;
        }

        private void Awake()
        {
            InjectDependencies("InjectHighScoreView");
        }

        public void SetHighScore(HighScore highScore)
        {
            textView.text = string.Format(highScoreFormat, highScore.Name, highScore.ScorePoints);
        }
    }
}