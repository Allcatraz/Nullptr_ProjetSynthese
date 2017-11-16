using UnityEngine;
using Harmony;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/InstanciateOnDeath")]
    public class InstanciateOnDeath : GameScript
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Inventory inventory;

        private void InjectInstanciateOnDeath([EventChannelScope] PlayerDeathEventChannel playerDeathEventChannel,
                                            [EntityScope] Inventory inventory)
        {
            this.playerDeathEventChannel = playerDeathEventChannel;
            this.inventory = inventory;
        }

        private void Awake()
        {
            InjectDependencies("InjectInstanciateOnDeath");
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnEventPublished += OnPlayerDeath;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnEventPublished -= OnPlayerDeath;
        }

        private void OnPlayerDeath(PlayerDeathEvent newEvent)
        {
            inventory.DropAll();
        }
    }
}