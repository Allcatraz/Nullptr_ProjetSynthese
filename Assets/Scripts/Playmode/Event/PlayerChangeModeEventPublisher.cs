using Harmony;

namespace ProjetSynthese
{
    public class PlayerChangeModeEventPublisher : GameScript
    {
        private PlayerController playerController;
        private PlayerChangeModeEventChannel playerChangeModeEventChannel;

        private void InjectPlayerChangeModeEventPublisher([GameObjectScope] PlayerController playerController,
                                                          [EventChannelScope] PlayerChangeModeEventChannel playerChangeModeEventChannel)
        {
            this.playerController = playerController;
            this.playerChangeModeEventChannel = playerChangeModeEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerChangeModeEventPublisher");
        }

        private void OnEnable()
        {
            playerController.OnChangeMode += OnPlayerChangeMode;
        }

        private void OnDisable()
        {
            playerController.OnChangeMode -= OnPlayerChangeMode;
        }

        private void OnPlayerChangeMode(bool isPlayerInFirstPerson)
        {
            playerChangeModeEventChannel.Publish(new PlayerChangeModeEvent(isPlayerInFirstPerson));
        }
    }
}
