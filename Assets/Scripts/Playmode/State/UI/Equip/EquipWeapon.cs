using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace ProjetSynthese
{
    public class EquipWeapon : GameScript
    {
        [Tooltip("Nom de l'object")]
        [SerializeField]
        private Text textName;

        [Tooltip("Text du slot d'interface.")]
        [SerializeField]
        private Text textSlot;

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
            UpdateText(newEvent.Inventory);
        }

        private void UpdateText(Inventory inventory)
        {
            Weapon equipped = inventory.Parent.GetComponent<PlayerController>().GetCurrentWeapon();
            textSlot.text = "";
            string name = "";
            if (equipped != null)
            {
                if (inventory.GetPrimaryWeapon().GetItem() == equipped)
                {
                    textSlot.text = EquipWeaponAt.Primary.ToString();
                }
                else if (inventory.GetSecondaryWeapon().GetItem() == equipped)
                {
                    textSlot.text = EquipWeaponAt.Secondary.ToString();
                }
                name = equipped.Type.ToString();
            }
            textName.text = name;  
        }

        private void UpdateImage()
        {

        }
    }
}


