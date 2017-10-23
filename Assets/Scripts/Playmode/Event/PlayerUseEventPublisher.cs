using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerUseEventPublisher")]
    public class PlayerUseEventPublisher : GameScript
    {
        private PlayerController playerController;
        private PlayerUseEventChannel playerUseEventChannel;

        private void InjectPlayerUseEventPublisher([GameObjectScope] PlayerController playerController,
                                                   [EventChannelScope] PlayerUseEventChannel playerUseEventChannel)
        {
            this.playerController = playerController;
            this.playerUseEventChannel = playerUseEventChannel;
        }

        // Use this for initialization
        private void Awake()
        {
            InjectDependencies("InjectPlayerUseEventPublisher");
        }

        private void OnEnable()
        {
            playerController.OnUse += OnUse;
        }

        private void OnDisable()
        {
            playerController.OnUse -= OnUse;
        }

        private void OnUse(bool isDoingSomething)
        {
            playerUseEventChannel.Publish(new PlayerUseEvent(isDoingSomething));
        }
    }
}

