using UnityEngine;
using Harmony;
using Prototype.NetworkLobby;

namespace ProjetSynthese
{
    public class EndGameUIController : GameScript
    {
        PlayerDeathEventChannel playerDeathChannel;

        private void InjectEndGameUIController([EventChannelScope] PlayerDeathEventChannel playerDeathChannel)
        {
            this.playerDeathChannel = playerDeathChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectEndGameUIController");
            playerDeathChannel.OnEventPublished += OnEnd;
        }

        private void OnEnd(PlayerDeathEvent playerDeathEvent)
        {

        }

        private void OnDestroy()
        {
            playerDeathChannel.OnEventPublished -= OnEnd;
        }
    }
}