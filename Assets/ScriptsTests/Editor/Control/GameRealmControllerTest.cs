using System.Collections;
using Harmony;
using Harmony.EventSystem;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class GameRealmControllerTest : UnitTestCase
    {
        private const uint InitialNumberOfRocks = 2;
        private const uint SpawnTimerInSeconds = 2;
        private const uint NumberOfRocksForSpawnToActivate = 5;

        private PlayerSpawner playerSpawner;
        private RockSpawner rockSpawner;
        private PrefabInstanceCollection rockCollection;
        private ICoroutineExecutor coroutineExecutor;
        private ICoroutine coroutine;
        private Score score;
        private GameEventChannel gameEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private GameFragmentController gameFragmentController;

        private IEnumerator coroutineEnumetator;

        [SetUp]
        public void Before()
        {
            playerSpawner = CreateSubstitute<PlayerSpawner>();
            rockSpawner = CreateSubstitute<RockSpawner>();
            rockCollection = CreateSubstitute<PrefabInstanceCollection>();
            coroutineExecutor = CreateSubstitute<ICoroutineExecutor>();
            coroutine = CreateSubstitute<ICoroutine>();
            score = CreateSubstitute<Score>();
            gameEventChannel = CreateSubstitute<GameEventChannel>();
            playerDeathEventChannel = CreateSubstitute<PlayerDeathEventChannel>();
            gameFragmentController = CreateBehaviour<GameFragmentController>();

            rockCollection.Count.Returns(0);
            //Capture the Coroutine, so we can execute it manualy during the test.
            coroutineExecutor.StartCoroutine(Arg.Is(gameFragmentController),
                                             Arg.Do<IEnumerator>(arg => coroutineEnumetator = arg)).Returns(coroutine);
        }

        [Test]
        public void WhenStartingRegistersToEvents()
        {
            Initialize();

            playerDeathEventChannel.Received().OnEventPublished += Arg.Any<EventChannelHandler<PlayerDeathEvent>>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            playerDeathEventChannel.Received().OnEventPublished -= Arg.Any<EventChannelHandler<PlayerDeathEvent>>();
        }

        [Test]
        public void WhenResumeSpawnInitialRocks()
        {
            Initialize();

            CheckRocksSpawned(InitialNumberOfRocks);
        }

        [Test]
        public void WhenResumeSpanwPlayer()
        {
            Initialize();

            CheckPlayerSpanwed();
        }

        [Test]
        public void WhenResumeStartCreatingRocksAtInterval()
        {
            Initialize();

            CheckCoroutineStarted();
        }

        [Test]
        public void WhenResumePublishGameStartEvent()
        {
            Initialize();

            CheckGameStartEventPublished();
        }

        [Test]
        public void WhenDestroyedStopCreatingRocksAtInterval()
        {
            Initialize();

            Destroy();

            CheckCoroutineStoped();
        }

        [Test]
        public void SpawnRockWhenTimerEndsIfRockNumberIsBellowMax()
        {
            Initialize();

            MakeRockNumber(NumberOfRocksForSpawnToActivate - 1);
            MakeTimePass();

            CheckRocksSpawned(1);
        }

        [Test]
        public void DoNotpawnRockWhenTimerEndsIfRockNumberIsHigherThanMax()
        {
            Initialize();

            MakeRockNumber(NumberOfRocksForSpawnToActivate + 1);
            MakeTimePass();

            CheckRocksNotSpawned(1);
        }

        [Test]
        public void WhenPlayerDiePublishGameEndEvent()
        {
            Initialize();

            MakePlayerDie();

            CheckGameEndEventPublished();
        }

        private void Initialize()
        {
            gameFragmentController.InjectGameController(InitialNumberOfRocks,
                                                SpawnTimerInSeconds,
                                                NumberOfRocksForSpawnToActivate,
                                                playerSpawner,
                                                rockSpawner,
                                                score,
                                                rockCollection,
                                                coroutineExecutor,
                                                gameEventChannel,
                                                playerDeathEventChannel);
            gameFragmentController.Awake();
            gameFragmentController.OnCreate();
        }

        private void Destroy()
        {
            gameFragmentController.OnDestroy();
        }

        private void MakeRockNumber(uint amountOfRocks)
        {
            rockCollection.Count.Returns((int) amountOfRocks);
            rockCollection.OnInstanceRemoved += Raise.Event<PrefabInstanceRemovedEventHandler>(R.E.Prefab.Rock, CreateGameObject());
        }

        private void CheckGameStartEventPublished()
        {
            gameEventChannel.Received().Publish(Arg.Is<GameEvent>(gameEvent => !gameEvent.HasGameEnded && gameEvent.Score == score));
        }

        private void CheckGameEndEventPublished()
        {
            gameEventChannel.Received().Publish(Arg.Is<GameEvent>(gameEvent => gameEvent.HasGameEnded && gameEvent.Score == score));
        }

        private void CheckPlayerSpanwed()
        {
            playerSpawner.Received().Spawn();
        }

        private void CheckRocksSpawned(uint nbOfRocks)
        {
            rockSpawner.Received().SpawnNormals(nbOfRocks);
        }

        private void CheckRocksNotSpawned(uint nbOfRocks)
        {
            rockSpawner.Received(0).SpawnNormals(nbOfRocks);
        }

        private void CheckCoroutineStarted()
        {
            coroutineExecutor.Received().StartCoroutine(gameFragmentController, Arg.Any<IEnumerator>());
        }

        private void CheckCoroutineStoped()
        {
            coroutine.Received().Stop();
        }

        private void MakeTimePass()
        {
            coroutineEnumetator.MoveNext();
        }

        private void MakePlayerDie()
        {
            playerDeathEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<PlayerDeathEvent>>(new PlayerDeathEvent());
        }
    }
}