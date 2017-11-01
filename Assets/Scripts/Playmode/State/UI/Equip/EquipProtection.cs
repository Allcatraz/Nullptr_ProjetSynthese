using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace ProjetSynthese
{
    public class EquipProtection : GameScript
    {
        [Tooltip("Le type de l'item.")]
        [SerializeField]
        private ItemType type;

        [Tooltip("L'image représentant l'item.")]
        [SerializeField]
        private Image image;

        private InventoryChangedEventChannel inventoryChangedEventChannel;


        private void InjectInventoryChange([EventChannelScope] InventoryChangedEventChannel inventoryChangedEventChannel)
        {
            this.inventoryChangedEventChannel = inventoryChangedEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectInventoryChange");
            inventoryChangedEventChannel.OnEventPublished += InventoryChangedEventChannel_OnEventPublished; ;
        }

        private void InventoryChangedEventChannel_OnEventPublished(InventoryChangeEvent newEvent)
        {
            UpdateImage(newEvent.Inventory);
        }

        private void UpdateImage(Inventory inventory)
        {
            ObjectContainedInventory vestCell = inventory.GetVest();
            ObjectContainedInventory helmetCell = inventory.GetHelmet();
            if (type == ItemType.Vest && vestCell != null)
            {
                Item vest = vestCell.GetItem();
            }
            if (type == ItemType.Helmet && helmetCell != null)
            {
                Item helmet = helmetCell.GetItem();
            }
        }

    }
}

