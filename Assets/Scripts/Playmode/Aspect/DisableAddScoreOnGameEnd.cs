using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/DisableAddScoreOnGameEnd")]
    public class DisableAddScoreOnGameEnd : GameScript
    {
        private AddScoreOnPrefabDeath addScoreOnPrefabDeath;
        private GameEventChannel gameEventChannel;

        private void InjectDisableAddScoreOnPlayerDeath([GameObjectScope] AddScoreOnPrefabDeath addScoreOnPrefabDeath,
                                                       [EventChannelScope] GameEventChannel gameEventChannel)
        {
            this.addScoreOnPrefabDeath = addScoreOnPrefabDeath;
            this.gameEventChannel = gameEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDisableAddScoreOnPlayerDeath");
        }

        private void OnEnable()
        {
            gameEventChannel.OnEventPublished += OnGameStateChanged;
        }

        private void OnDisable()
        {
            gameEventChannel.OnEventPublished -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameEvent gameEvent)
        {
            if (gameEvent.HasGameEnded)
            {
                addScoreOnPrefabDeath.enabled = false;
            }
        }
    }
}