using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class DroppedItemController : GameScript
    {
        private SpawnItemDropEventChannel spawnItemDropEventChannel;

        [SerializeField] private GameObject spawnLocation;

        private void Awake()
        {
            InjectDependencies("InjectEventSensor");
            spawnItemDropEventChannel.OnEventPublished += SpawnItemDropEventChannel_OnEventPublished;
        }

        private void SpawnItemDropEventChannel_OnEventPublished(SpawnItemDropEvent newEvent)
        {
            SpawnItem(newEvent.ItemToSpawn.GetItem().gameObject, newEvent.ItemToSpawn.GetItem().Player.transform);
        }

        private void InjectEventSensor([EventChannelScope] SpawnItemDropEventChannel spawnItemDropEventChannel)
        {
            this.spawnItemDropEventChannel = spawnItemDropEventChannel;
        }

        private void SpawnItem(GameObject itemToSpawn, Transform player)
        {
            itemToSpawn.SetActive(true);
            itemToSpawn.transform.SetParent(spawnLocation.transform, false);
            itemToSpawn.transform.position = player.position;
        }
    }
}


