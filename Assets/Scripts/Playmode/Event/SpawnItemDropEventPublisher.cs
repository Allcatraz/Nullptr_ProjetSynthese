using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/SpawnItemDropEventPublisher")]
    public class SpawnItemDropEventPublisher : GameScript
    {
        private Inventory itemToSpawn;
        private SpawnItemDropEventChannel spawnItemDropEventChannel;

        private void InjectSpawnItemDropEventPublisher([EntityScope] Inventory itemToSpawn,
                                                    [EventChannelScope] SpawnItemDropEventChannel spawnItemDropEventChannel)
        {
            this.itemToSpawn = itemToSpawn;
            this.spawnItemDropEventChannel = spawnItemDropEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectSpawnItemDropEventPublisher");
        }

        private void OnEnable()
        {
            itemToSpawn.SpawnItem += ItemToSpawn_SpawnItem;
        }

        private void ItemToSpawn_SpawnItem(ObjectContainedInventory itemToSpawn)
        {
            spawnItemDropEventChannel.Publish(new SpawnItemDropEvent(itemToSpawn));
        }

        private void OnDisable()
        {
            itemToSpawn.SpawnItem -= ItemToSpawn_SpawnItem;
        }
    }
}

