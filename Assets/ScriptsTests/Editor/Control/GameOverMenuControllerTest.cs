using Harmony;
using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class GameOverMenuControllerTest : UnitTestCase
    {
        private Score score;
        private UnityMenu newHighScoreMenu;
        private ISelectable retryButton;
        private HighScoreRepository highScoreRepository;
        private IActivityStack activityStack;
        private IMenuStack menuStack;
        private GameOverMenuController gameOverMenuController;

        [SetUp]
        public void Before()
        {
            score = CreateSubstitute<Score>();
            newHighScoreMenu = CreateSubstitute<UnityMenu>();
            retryButton = CreateSubstitute<ISelectable>();
            highScoreRepository = CreateSubstitute<HighScoreRepository>();
            activityStack = CreateSubstitute<IActivityStack>();
            menuStack = CreateSubstitute<IMenuStack>();
            gameOverMenuController = CreateBehaviour<GameOverMenuController>();
        }

        [Test]
        public void WhenResumedRetryButtonIsSelected()
        {
            Initialize();

            gameOverMenuController.OnResume();

            CheckRetryButtonSelected();
        }

        [Test]
        public void ShowNewHighScoreLowestScoreIsNull()
        {
            Initialize();

            MakeLowestScoreNull();
            gameOverMenuController.OnCreate(new GameEvent(true, score));

            CheckNewHighScoreShown();
        }

        [Test]
        public void ShowNewHighScoreLeaderboardIsNotFull()
        {
            Initialize();

            MakeLeaderboardNotFull(10u);
            score.ScorePoints.Returns(100u);
            gameOverMenuController.OnCreate(new GameEvent(true, score));

            CheckNewHighScoreShown();
        }

        [Test]
        public void ShowNewHighScoreIfScoreIsHigherThanLowest()
        {
            Initialize();

            MakeLowestScore(10);
            score.ScorePoints.Returns(100u);
            gameOverMenuController.OnCreate(new GameEvent(true, score));

            CheckNewHighScoreShown();
        }

        [Test]
        public void DoNotShowNewHighScoreIfScoreIsLowerThanLowest()
        {
            Initialize();

            MakeLowestScore(100);
            score.ScorePoints.Returns(10u);
            gameOverMenuController.OnCreate(new GameEvent(true, score));

            CheckNewHighScoreNotShown();
        }

        [Test]
        public void RestartActivityOnRetry()
        {
            Initialize();

            gameOverMenuController.RetryGame();

            CheckActivityRestarted();
        }

        [Test]
        public void StopActivityOnQuit()
        {
            Initialize();

            gameOverMenuController.QuitGame();

            CheckActivityStopped();
        }

        private void Initialize()
        {
            gameOverMenuController.InjectGameOverController(newHighScoreMenu,
                                                        retryButton,
                                                        highScoreRepository,
                                                        activityStack,
                                                        menuStack);
            gameOverMenuController.Awake();
        }

        private void CheckRetryButtonSelected()
        {
            retryButton.Received().Select();
        }

        private void MakeLowestScoreNull()
        {
            highScoreRepository.GetLowestHighScore().Returns((HighScore) null);
            highScoreRepository.IsLeaderboardFull().Returns(false);
        }

        private void MakeLowestScore(uint scorePoints)
        {
            highScoreRepository.GetLowestHighScore().Returns(new HighScore {ScorePoints = scorePoints});
            highScoreRepository.IsLeaderboardFull().Returns(true);
        }

        private void MakeLeaderboardNotFull(uint lowestScorePoints)
        {
            highScoreRepository.GetLowestHighScore().Returns(new HighScore { ScorePoints = lowestScorePoints });
            highScoreRepository.IsLeaderboardFull().Returns(false);
        }

        private void CheckNewHighScoreShown()
        {
            menuStack.Received().StartMenu(newHighScoreMenu, score);
        }

        private void CheckNewHighScoreNotShown()
        {
            menuStack.Received(0).StartMenu(newHighScoreMenu, score);
        }

        private void CheckActivityRestarted()
        {
            activityStack.Received().RestartCurrentActivity();
        }

        private void CheckActivityStopped()
        {
            activityStack.Received().StopCurrentActivity();
        }
    }
}