using Harmony;
using UnityEngine;


namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/InventoryChangeEventPublisher")]
    public class InventoryChangeEventPublisher : GameScript
    {
        private Inventory inventory;
        private InventoryChangedEventChannel inventoryChangedEventChannel;

        private void InjectInventoryChangeEventPublisher([EntityScope] Inventory inventory,
                                                    [EventChannelScope] InventoryChangedEventChannel inventoryChangedEventChannel)
        {
            this.inventory = inventory;
            this.inventoryChangedEventChannel = inventoryChangedEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectInventoryChangeEventPublisher");
        }

        private void OnEnable()
        {
            inventory.InventoryChange += Inventory_InventoryChange;
        }

        private void Inventory_InventoryChange()
        {
            inventoryChangedEventChannel.Publish(new InventoryChangeEvent(inventory));
        }

        private void OnDisable()
        {
            inventory.InventoryChange -= Inventory_InventoryChange;
        }
    }
}

