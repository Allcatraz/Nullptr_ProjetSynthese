using System.Collections;
using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Control/GameFragmentController")]
    public class GameFragmentController : GameScript, IFragmentController
    {
        [SerializeField]
        private uint initialNumberOfRocks;

        [SerializeField]
        private uint spawnTimerInSeconds;

        [SerializeField]
        private uint numberOfRocksForSpawnToActivate;

        private PlayerSpawner playerSpawner;
        private RockSpawner rockSpawner;
        private Score score;
        private PrefabInstanceCollection rockCollection;
        private ICoroutineExecutor coroutineExecutor;
        private GameEventChannel gameEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;

        private ICoroutine spawnCoroutine;

        public void InjectGameController(uint initialNumberOfRocks,
                                         uint spawnTimerInSeconds,
                                         uint numberOfRocksForSpawnToActivate,
                                         [SceneScope] PlayerSpawner playerSpawner,
                                         [SceneScope] RockSpawner rockSpawner,
                                         [SceneScope] Score score,
                                         [Named(R.S.GameObject.RockCollection)] [SceneScope] PrefabInstanceCollection rockCollection,
                                         [ApplicationScope] ICoroutineExecutor coroutineExecutor,
                                         [EventChannelScope] GameEventChannel gameEventChannel,
                                         [EventChannelScope] PlayerDeathEventChannel playerDeathEventChannel)
        {
            this.initialNumberOfRocks = initialNumberOfRocks;
            this.spawnTimerInSeconds = spawnTimerInSeconds;
            this.numberOfRocksForSpawnToActivate = numberOfRocksForSpawnToActivate;
            this.playerSpawner = playerSpawner;
            this.rockSpawner = rockSpawner;
            this.score = score;
            this.rockCollection = rockCollection;
            this.coroutineExecutor = coroutineExecutor;
            this.gameEventChannel = gameEventChannel;
            this.playerDeathEventChannel = playerDeathEventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectGameController",
                               initialNumberOfRocks,
                               spawnTimerInSeconds,
                               numberOfRocksForSpawnToActivate);

            playerDeathEventChannel.OnEventPublished += OnPlayerDeath;
        }

        public void OnCreate()
        {
            StartGame();
        }

        public void OnStop()
        {
            //Nothing to do
        }

        public void OnDestroy()
        {
            StopSpawningRocks();

            playerDeathEventChannel.OnEventPublished -= OnPlayerDeath;
        }

        private void StartGame()
        {
            SpawnInitialRocks();
            StartSpawningRocks();
            SpawnPlayer();

            NotifyGameState(false);
        }

        private void StopGame()
        {
            StopSpawningRocks();

            NotifyGameState(true);
        }

        private void SpawnInitialRocks()
        {
            rockSpawner.SpawnNormals(initialNumberOfRocks);
        }

        private void SpawnPlayer()
        {
            playerSpawner.Spawn();
        }

        private void StartSpawningRocks()
        {
            spawnCoroutine = coroutineExecutor.StartCoroutine(this, SpawnRocksAtInterval());
        }

        private void StopSpawningRocks()
        {
            if (spawnCoroutine != null)
            {
                spawnCoroutine.Stop();
                spawnCoroutine = null;
            }
        }

        private IEnumerator SpawnRocksAtInterval()
        {
            while (true)
            {
                if (rockCollection.Count < numberOfRocksForSpawnToActivate)
                {
                    rockSpawner.SpawnNormals(1);
                }
                yield return new WaitForSeconds(spawnTimerInSeconds);
            }
        }

        private void OnPlayerDeath(PlayerDeathEvent playerDeathEvent)
        {
            StopGame();
        }

        private void NotifyGameState(bool hasGameEnded)
        {
            gameEventChannel.Publish(new GameEvent(hasGameEnded, score));
        }
    }
}