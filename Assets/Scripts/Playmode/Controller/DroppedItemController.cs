using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class DroppedItemController : GameScript
    {
        public static DroppedItemController Instance { get; set; }

        private SpawnItemDropEventChannel spawnItemDropEventChannel;

        [Tooltip("La location de spawn de l'objet passé en paramètre")]
        [SerializeField] private GameObject spawnLocation;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject SpawnItemDropEventChannel(SpawnItemDropEvent newEvent)
        {
            if (newEvent.ItemToSpawn.GetItem() != null && newEvent.ItemToSpawn.GetItem().Player != null)
            {
                return SpawnItem(newEvent.ItemToSpawn.GetItem().gameObject, newEvent.ItemToSpawn.GetItem().Player.transform.position);
            }

            return null;
        }

        private GameObject SpawnItem(GameObject itemToSpawn, Vector3 playerPos)
        {
            GameObject newItem = Instantiate(itemToSpawn);
            SetAmmoTypeInNewObjectIfIsAmmoPack(itemToSpawn, newItem);
            newItem.GetComponent<Item>().Level = itemToSpawn.GetComponent<Item>().Level;
            ChangeLayerOfItemAndChildrenTo(newItem, R.S.Layer.Item);
            newItem.transform.position = playerPos;
            newItem.SetActive(true);
            return newItem;
        }

        private void SetAmmoTypeInNewObjectIfIsAmmoPack(GameObject itemToSpawn, GameObject newItem)
        {
            Item toSpawn = itemToSpawn.GetComponent<Item>();
            Item spawner = newItem.GetComponent<Item>();
            if (toSpawn != null && spawner != null)
            {
                if (toSpawn as AmmoPack && spawner as AmmoPack)
                {
                    (spawner as AmmoPack).AmmoType = (toSpawn as AmmoPack).AmmoType;
                }
            }
        }

        private void ChangeLayerOfItemAndChildrenTo(GameObject itemToChangeLayer, string layer)
        {
            itemToChangeLayer.layer = LayerMask.NameToLayer(layer);
            List<GameObject> allItems = itemToChangeLayer.gameObject.GetAllChildrens().ToList();
            allItems.ForEach(obj => obj.layer = LayerMask.NameToLayer(layer));
        }
    }
}