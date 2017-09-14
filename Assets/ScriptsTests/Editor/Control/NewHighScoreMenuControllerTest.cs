using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class NewHighScoreMenuControllerTest : UnitTestCase
    {
        private const string Name = "BEN";
        private const uint ScorePoints = 100;

        private Score score;
        private ITextInput nameInput;
        private ISelectable okButton;
        private IMenuStack menuStack;
        private HighScoreRepository highScoreRepository;
        private NewHighScoreMenuController newHighScoreMenuController;

        [SetUp]
        public void Before()
        {
            score = CreateSubstitute<Score>();
            nameInput = CreateSubstitute<ITextInput>();
            okButton = CreateSubstitute<ISelectable>();
            menuStack = CreateSubstitute<IMenuStack>();
            highScoreRepository = CreateSubstitute<HighScoreRepository>();
            newHighScoreMenuController = CreateBehaviour<NewHighScoreMenuController>();

            nameInput.Text.Returns(Name);
            score.ScorePoints.Returns(ScorePoints);
        }

        [Test]
        public void WhenResumedSelectNameInput()
        {
            Initialize();

            newHighScoreMenuController.OnResume();

            CheckNameInputSelected();
        }

        [Test]
        public void WhenClickOkAddScoreToDatabase()
        {
            Initialize();

            newHighScoreMenuController.OnCreate(score);
            newHighScoreMenuController.SaveHighScore();

            CheckHighScoreSaved();
        }

        [Test]
        public void WhenClickOkStopCurrentMenu()
        {
            Initialize();

            newHighScoreMenuController.OnCreate(score);
            newHighScoreMenuController.SaveHighScore();

            CheckCurrentMenuStopped();
        }

        [Test]
        public void SelectOkButtonOnEditEnd()
        {
            Initialize();

            newHighScoreMenuController.EndScoreEdit();

            CheckOkButtonSelected();
        }

        private void Initialize()
        {
            newHighScoreMenuController.InjectNewHighScoreController(nameInput,
                                                                    okButton,
                                                                    menuStack,
                                                                    highScoreRepository);
            newHighScoreMenuController.Awake();
        }

        private void CheckNameInputSelected()
        {
            nameInput.Received().Select();
        }

        private void CheckOkButtonSelected()
        {
            okButton.Received().Select();
        }

        private void CheckHighScoreSaved()
        {
            highScoreRepository.Received().AddScore(Arg.Is<HighScore>(highScore =>
                                                                          highScore.Name == Name &&
                                                                          highScore.ScorePoints == ScorePoints));
        }

        private void CheckCurrentMenuStopped()
        {
            menuStack.Received().StopCurrentMenu();
        }
    }
}