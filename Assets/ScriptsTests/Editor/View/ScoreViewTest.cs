using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class ScoreViewTest : UnitTestCase
    {
        private ITextView textView;
        private ScoreView scoreView;

        [SetUp]
        public void Before()
        {
            textView = CreateSubstitute<ITextView>();
            scoreView = CreateBehaviour<ScoreView>();
        }

        [Test]
        public void CanChangeScore()
        {
            Initialize(5);

            scoreView.SetScore(5);

            CheckScoreIs("00005");
        }

        private void Initialize(uint nbZeros)
        {
            scoreView.InjectScoreView(nbZeros, textView);
            scoreView.Awake();
        }

        private void CheckScoreIs(string score)
        {
            textView.Received().Text = score;
        }
    }
}