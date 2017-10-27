using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    //BEN_CORRECTION : J'en reviens encore à ça : ceci n'est pas un contrôleur.
    //
    //                 On dirait que cette classe devrait juste être un intermédaire entre le EventChannel 
    //                 et une fabrique.
    //
    //                 En fait, je me demande même si l'usage d'un canal d'événements (soit SpawnItemDropEventChannel) 
    //                 est justifié. Primo, ce script est le seul à utiliser le canal, et secundo, Inventory
    //                 serait très légitimé de l'appeller directement.
    //
    //                 Autrement dit, ce script servirait uniquement à "Spawner" un item sur le sol,
    //                 une sorte de "ItemSpawner"...
    //
    //                 Ce qui me fait penser qu'il existe déjà "ItemSpawnerController".
    //
    //                 Bref, cette classe doit être retravaillée.
    
    public class DroppedItemController : GameScript
    {
        private SpawnItemDropEventChannel spawnItemDropEventChannel;

        //BEN_REVIEW : http://www.larousse.fr/dictionnaires/francais/location/47601
        [Tooltip("La location de spawn de l'objet passé en paramètre")]
        [SerializeField] private GameObject spawnLocation;

        private void Awake()
        {
            InjectDependencies("InjectEventSensor");
            spawnItemDropEventChannel.OnEventPublished += SpawnItemDropEventChannel_OnEventPublished;
        }

        //BEN_CORRECTION : Nommage. OnSpawnItem ?
        private void SpawnItemDropEventChannel_OnEventPublished(SpawnItemDropEvent newEvent)
        {
            //BEN_REVIEW : Pourquoi Item serait null ? Dans quel cas ? Est-ce vraiment une bonne idée ?
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
            //BEN_CORRECTION : N'avez-vous pas des fabriques? Pourquoi êtes-vous obligés de faire cela ainsi ?
            
            GameObject newItem = Instantiate(itemToSpawn);
            SetAmmoTypeInNewObjectIfIsAmmoPack(itemToSpawn, newItem);
            newItem.GetComponent<Item>().Level = itemToSpawn.GetComponent<Item>().Level;
            ChangeLayerOfItemAndChildrenTo(newItem, R.S.Layer.Item);
            newItem.transform.position = player.position;
            newItem.SetActive(true);
            NetworkServer.Spawn(newItem);
        }

        private void SetAmmoTypeInNewObjectIfIsAmmoPack(GameObject itemToSpawn, GameObject newItem)
        {
            Item toSpawn = itemToSpawn.GetComponent<Item>();
            Item spawner = newItem.GetComponent<Item>();
            if (toSpawn != null && spawner != null)
            {
                //BEN_CORRECTION : "is", pas "as".
                if (toSpawn as AmmoPack && spawner as AmmoPack)
                {
                    (spawner as AmmoPack).AmmoType = (toSpawn as AmmoPack).AmmoType;
                }
            }
        }

        //BEN_CORRECTION : Pourquoi ? Ça sert à quoi ? Est-ce vraiment de la responsabilité de cette classe ?
        private void ChangeLayerOfItemAndChildrenTo(GameObject itemToChangeLayer, string layer)
        {
            itemToChangeLayer.layer = LayerMask.NameToLayer(layer);
            List<GameObject> allItems = itemToChangeLayer.gameObject.GetAllChildrens().ToList();
            allItems.ForEach(obj => obj.layer = LayerMask.NameToLayer(layer));
        }
    }
}


