using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class HighScoreViewTest : UnitTestCase
    {
        private const string HighScoreFormat = "{0} : {1}";

        private ITextView textView;
        private HighScoreView highScoreView;

        [SetUp]
        public void Before()
        {
            textView = CreateSubstitute<ITextView>();
            highScoreView = CreateBehaviour<HighScoreView>();
        }

        [Test]
        public void ShowScoreOnTextViewWithNameAndScore()
        {
            Initialize();

            highScoreView.SetHighScore(new HighScore {Name = "Bob", ScorePoints = 100});
            CheckTextShownIs("Bob : 100");

            highScoreView.SetHighScore(new HighScore {Name = "Benjamin", ScorePoints = 20000});
            CheckTextShownIs("Benjamin : 20000");
        }

        private void Initialize()
        {
            highScoreView.InjectHighScoreView(HighScoreFormat, textView);
            highScoreView.Awake();
        }

        private void CheckTextShownIs(string text)
        {
            textView.Received().Text = text;
        }
    }
}