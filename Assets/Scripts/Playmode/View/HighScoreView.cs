using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/View/HighScoreView")]
    public class HighScoreView : GameScript
    {
        [SerializeField]
        private string highScoreFormat  = "{0} : {1}";

        private ITextView textView;

        public void InjectHighScoreView(string highScoreFormat,
                                        [GameObjectScope] ITextView textView)
        {
            this.highScoreFormat = highScoreFormat;
            this.textView = textView;
        }

        public void Awake()
        {
            InjectDependencies("InjectHighScoreView", highScoreFormat);
        }

        public void SetHighScore(HighScore highScore)
        {
            textView.Text = string.Format(highScoreFormat, highScore.Name, highScore.ScorePoints);
        }
    }
}