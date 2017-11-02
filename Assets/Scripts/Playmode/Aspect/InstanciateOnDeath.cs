using UnityEngine;
using Harmony;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/InstanciateOnDeath")]
    public class InstanciateOnDeath : GameScript
    {
        [Tooltip("Prefab de l'objet représentant l'inventaire dans le monde")]
        [SerializeField]
        private GameObject cratePrefab;

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
            GameObject crate = Instantiate(cratePrefab);
            crate.transform.position = inventory.Parent.transform.position;
            NetworkServer.Spawn(crate);
        }
    }
}