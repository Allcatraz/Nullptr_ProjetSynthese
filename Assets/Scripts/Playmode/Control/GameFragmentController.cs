using System.Collections;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/GameFragmentController")]
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
        private GameEventChannel gameEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;

        private Coroutine spawnCoroutine;

        private void InjectGameController([SceneScope] PlayerSpawner playerSpawner,
                                         [SceneScope] RockSpawner rockSpawner,
                                         [SceneScope] Score score,
                                         [Named(R.S.GameObject.RockCollection)] [SceneScope] PrefabInstanceCollection rockCollection,
                                         [EventChannelScope] GameEventChannel gameEventChannel,
                                         [EventChannelScope] PlayerDeathEventChannel playerDeathEventChannel)
        {
            this.playerSpawner = playerSpawner;
            this.rockSpawner = rockSpawner;
            this.score = score;
            this.rockCollection = rockCollection;
            this.gameEventChannel = gameEventChannel;
            this.playerDeathEventChannel = playerDeathEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectGameController");

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

        private void OnDestroy()
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
            spawnCoroutine = StartCoroutine(SpawnRocksAtIntervalRoutine());
        }

        private void StopSpawningRocks()
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }

        private IEnumerator SpawnRocksAtIntervalRoutine()
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