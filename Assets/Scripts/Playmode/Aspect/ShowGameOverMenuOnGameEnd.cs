using Harmony;
using Harmony.Injection;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Aspect/ShowGameOverMenuOnGameEnd")]
    public class ShowGameOverMenuOnGameEnd : GameScript
    {
        [SerializeField]
        private UnityMenu gameOverMenu;

        private IMenuStack menuStack;
        private GameEventChannel gameEventChannel;

        public void InjectShowGameOverMenuOnGameEnd(UnityMenu gameOverMenu,
                                                    [ApplicationScope] IMenuStack menuStack,
                                                    [EventChannelScope] GameEventChannel gameEventChannel)
        {
            this.gameOverMenu = gameOverMenu;
            this.menuStack = menuStack;
            this.gameEventChannel = gameEventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectShowGameOverMenuOnGameEnd",
                               gameOverMenu);
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
                menuStack.StartMenu(gameOverMenu, gameEvent);
            }
        }
    }
}