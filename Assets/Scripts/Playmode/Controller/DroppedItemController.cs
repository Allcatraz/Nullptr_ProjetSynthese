using Harmony;
using UnityEngine;
using UnityEngine.Networking;

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
            if (newEvent.ItemToSpawn.GetItem() != null && newEvent.ItemToSpawn.GetItem().Player != null)
            {
                SpawnItem(newEvent.ItemToSpawn.GetItem().gameObject, newEvent.ItemToSpawn.GetItem().Player.transform);
            }  
        }

        private void InjectEventSensor([EventChannelScope] SpawnItemDropEventChannel spawnItemDropEventChannel)
        {
            this.spawnItemDropEventChannel = spawnItemDropEventChannel;
        }

        private void SpawnItem(GameObject itemToSpawn, Transform player)
        {
            GameObject newItem = Instantiate(itemToSpawn);
            newItem.GetComponent<Item>().Level = itemToSpawn.GetComponent<Item>().Level;
            newItem.transform.SetParent(spawnLocation.transform, true);
            newItem.layer = LayerMask.NameToLayer(R.S.Layer.Item);
            newItem.transform.position = player.position;
            newItem.SetActive(true);
            NetworkServer.Spawn(newItem);
        }
    }
}


