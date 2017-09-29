using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class EquipWeapon : GameScript
    {
        [SerializeField]
        private EquipWeaponAt typeSlot;

        [SerializeField]
        private Text textName;

        [SerializeField]
        private Text textSlot;

        private Inventory inventory;

        private void UpdateInventory()
        {
            if (inventory != StaticInventoryPass.Inventory)
            {
                inventory = StaticInventoryPass.Inventory;
            } 
        }

        private void UpdateText()
        {
            textSlot.text = typeSlot.ToString();
            string name = "";
            if (typeSlot == EquipWeaponAt.Primary)
            {
                name = inventory.GetPrimaryWeapon().GetItem().Type.ToString();
            }
            if (typeSlot == EquipWeaponAt.Secondary)
            {
                name = inventory.GetSecondaryWeapon().GetItem().Type.ToString();
            }
            textName.text = name;
            
        }

        private void UpdateImage()
        {

        }

        void FixedUpdate()
        {
            UpdateInventory();
            UpdateText();
            UpdateImage();
        }
    }
}


