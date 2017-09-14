using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Aspect/DisableAddScoreOnGameEnd")]
    public class DisableAddScoreOnGameEnd : GameScript
    {
        private AddScoreOnPrefabDeath addScoreOnPrefabDeath;
        private GameEventChannel gameEventChannel;

        public void InjectDisableAddScoreOnPlayerDeath([GameObjectScope] AddScoreOnPrefabDeath addScoreOnPrefabDeath,
                                                       [EventChannelScope] GameEventChannel gameEventChannel)
        {
            this.addScoreOnPrefabDeath = addScoreOnPrefabDeath;
            this.gameEventChannel = gameEventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectDisableAddScoreOnPlayerDeath");
        }

        public void OnEnable()
        {
            gameEventChannel.OnEventPublished += OnGameStateChanged;
        }

        public void OnDisable()
        {
            gameEventChannel.OnEventPublished -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameEvent gameEvent)
        {
            if (gameEvent.HasGameEnded)
            {
                addScoreOnPrefabDeath.DisableScoreCount();
            }
        }
    }
}