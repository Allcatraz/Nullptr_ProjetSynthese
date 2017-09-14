using System.Collections.Generic;
using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class HighScoresMenuControllerTest : UnitTestCase
    {
        private static readonly IList<HighScore> HighScores = new List<HighScore>();

        private ISelectable okButton;
        private HighScoreRepository highScoreRepository;
        private HighScoreListView highScoreListView;
        private IMenuStack menuStack;
        private HighScoresMenuController highScoresMenuController;

        [SetUp]
        public void Before()
        {
            okButton = CreateSubstitute<ISelectable>();
            highScoreRepository = CreateSubstitute<HighScoreRepository>();
            highScoreListView = CreateSubstitute<HighScoreListView>();
            menuStack = CreateSubstitute<IMenuStack>();
            highScoresMenuController = CreateBehaviour<HighScoresMenuController>();

            highScoreRepository.GetAllHighScores().Returns(HighScores);
        }

        [Test]
        public void WhenShownShowsHighScores()
        {
            Initialize();

            CheckShowsAllHighScores();
        }

        [Test]
        public void WhenResumedSelectOkButton()
        {
            Initialize();

            CheckOkButtonSelected();
        }

        [Test]
        public void WhenClickOkExitActivity()
        {
            Initialize();

            highScoresMenuController.QuitHighScores();

            CheckMenuStop();
        }

        private void Initialize()
        {
            highScoresMenuController.InjectHighScoresController(okButton,
                                                            highScoreRepository,
                                                            highScoreListView,
                                                            menuStack);
            highScoresMenuController.Awake();
            highScoresMenuController.OnCreate();
            highScoresMenuController.OnResume();
        }

        private void CheckOkButtonSelected()
        {
            okButton.Received().Select();
        }

        private void CheckMenuStop()
        {
            menuStack.Received().StopCurrentMenu();
        }

        private void CheckShowsAllHighScores()
        {
            highScoreListView.Received().SetHighScores(HighScores);
        }
    }
}