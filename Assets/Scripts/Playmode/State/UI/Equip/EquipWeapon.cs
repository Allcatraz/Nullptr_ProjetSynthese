using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace ProjetSynthese
{
    public class EquipWeapon : GameScript
    {
        [Tooltip("Emplacement de l'image représentant l'arme equipper.")]
        [SerializeField]
        private Image icon;

        [SerializeField]
        private Sprite iconAwm;
        [SerializeField]
        private Sprite iconM16;
        [SerializeField]
        private Sprite iconMP5;
        [SerializeField]
        private Sprite iconSaiga;

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
            Weapon equipped = inventory.Parent.GetComponent<PlayerController>().GetCurrentWeapon();
            if (equipped != null)
            {
                if (equipped.Type == ItemType.AWM)
                {
                    icon.sprite = iconAwm;
                }
                else if (equipped.Type == ItemType.M16A4)
                {
                    icon.sprite = iconM16;
                }
                else if (equipped.Type == ItemType.MP5)
                {
                    icon.sprite = iconMP5;
                }
                else if (equipped.Type == ItemType.Saiga12)
                {
                    icon.sprite = iconSaiga;
                }
                else
                {
                    icon.sprite = null;
                }
            }
        }
    }
}


