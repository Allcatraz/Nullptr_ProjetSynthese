using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class HighScoreListViewTest : UnitTestCase
    {
        private GameObject contentView;
        private HighScoreViewSpawner highScoreViewSpawner;
        private HighScoreListView highScoreListView;

        [SetUp]
        public void Before()
        {
            contentView = CreateGameObject();
            highScoreViewSpawner = CreateSubstitute<HighScoreViewSpawner>();
            highScoreListView = CreateBehaviour<HighScoreListView>();
        }

        [Test]
        public void CreateHighScoreViewForEachView()
        {
            Initialize();

            IList<HighScore> highScores = new List<HighScore>
            {
                new HighScore(),
                new HighScore(),
                new HighScore()
            };
            highScoreListView.SetHighScores(highScores);

            CheckShownHighScore(highScores[0]);
            CheckShownHighScore(highScores[1]);
            CheckShownHighScore(highScores[2]);
        }

        private void Initialize()
        {
            highScoreListView.InjectHighScoresView(contentView,
                                                   highScoreViewSpawner);
            highScoreListView.Awake();
        }

        private void CheckShownHighScore(HighScore highScore)
        {
            highScoreViewSpawner.Received().Spawn(contentView, highScore);
        }
    }
}