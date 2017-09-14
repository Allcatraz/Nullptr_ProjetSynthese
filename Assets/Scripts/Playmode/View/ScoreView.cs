using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/View/ScoreView")]
    public class ScoreView : GameScript
    {
        private const string TextFormat = "D";

        [SerializeField]
        private uint nbZeros = 5;

        private ITextView textView;

        public void InjectScoreView(uint nbZeros,
                                    [GameObjectScope] ITextView textView)
        {
            this.nbZeros = nbZeros;
            this.textView = textView;
        }

        public void Awake()
        {
            InjectDependencies("InjectScoreView", nbZeros);
        }

        public virtual void SetScore(uint score)
        {
            textView.Text = score.ToString(TextFormat + nbZeros);
        }
    }
}