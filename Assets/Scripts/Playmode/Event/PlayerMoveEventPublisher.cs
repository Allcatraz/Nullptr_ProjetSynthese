using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerMoveEventPublisher")]
    public class PlayerMoveEventPublisher : GameScript
    {
        private PlayerMover playerMover;
        private PlayerMoveEventChannel playerMoveEventChannel;

        private void InjectPlayerMoveEventPublisher([EntityScope] PlayerMover playerMover,
                                                    [EventChannelScope] PlayerMoveEventChannel playerMoveEventChannel)
        {
            this.playerMover = playerMover;
            this.playerMoveEventChannel = playerMoveEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerMoveEventPublisher");
        }

        private void OnEnable()
        {
            playerMover.OnMove += OnMoveChanged;
        }

        private void OnDisable()
        {
            playerMover.OnMove -= OnMoveChanged;
        }

        private void OnMoveChanged()
        {
            playerMoveEventChannel.Publish(new PlayerMoveEvent(playerMover));
        }
    }
}
