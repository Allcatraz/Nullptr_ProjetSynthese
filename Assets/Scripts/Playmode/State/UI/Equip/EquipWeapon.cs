using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace ProjetSynthese
{
    public class EquipWeapon : GameScript
    {
        [Tooltip("Type de slot utilisé.")]
        [SerializeField]
        private EquipWeaponAt typeSlot;

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
            textSlot.text = typeSlot.ToString();
            string name = "";
            Weapon equipped = inventory.Parent.GetComponent<PlayerController>().GetCurrentWeapon();
            if (equipped != null)
            {
                name = equipped.Type.ToString();
            }
            textName.text = name;  
        }

        private void UpdateImage()
        {

        }
    }
}


