using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/View/ScoreView")]
    public class ScoreView : GameScript
    {
        private const string TextFormat = "D";

        [SerializeField]
        private uint nbZeros = 5;

        private Text textView;

        private void InjectScoreView([GameObjectScope] Text textView)
        {
            this.textView = textView;
        }

        private void Awake()
        {
            InjectDependencies("InjectScoreView");
        }

        public void SetScore(uint score)
        {
            textView.text = score.ToString(TextFormat + nbZeros);
        }
    }
}