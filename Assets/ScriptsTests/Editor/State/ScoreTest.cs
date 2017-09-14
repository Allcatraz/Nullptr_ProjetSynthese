using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class ScoreTest : UnitTestCase
    {
        private Score score;

        [SetUp]
        public void Before()
        {
            score = CreateBehaviour<Score>();
        }

        [Test]
        public void CanAddPointsToScore()
        {
            score.AddPoints(10);

            Assert.AreEqual(10, score.ScorePoints);
        }

        [Test]
        public void CanResetScoreToZero()
        {
            score.AddPoints(10);

            score.Reset();

            Assert.AreEqual(0, score.ScorePoints);
        }

        [Test]
        public void NotifyWhenScoreChanges()
        {
            ScoreChangedEventHandler scoreChangedEventHandler = CreateSubstitute<ScoreChangedEventHandler>();

            score.OnScoreChanged += scoreChangedEventHandler;
            score.AddPoints(10);

            scoreChangedEventHandler.Received()(0, 10);
        }
    }
}