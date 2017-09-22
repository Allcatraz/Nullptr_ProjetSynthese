using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/ShowGameOverMenuOnGameEnd")]
    public class ShowGameOverMenuOnGameEnd : GameScript
    {
        [SerializeField]
        private Menu gameOverMenu;

        private ActivityStack activityStack;
        private GameEventChannel gameEventChannel;

        private void InjectShowGameOverMenuOnGameEnd([ApplicationScope] ActivityStack activityStack,
                                                    [EventChannelScope] GameEventChannel gameEventChannel)
        {
            this.activityStack = activityStack;
            this.gameEventChannel = gameEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectShowGameOverMenuOnGameEnd");
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
                activityStack.StartMenu(gameOverMenu, gameEvent);
            }
        }
    }
}