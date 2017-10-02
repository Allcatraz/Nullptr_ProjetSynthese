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
            if (inventory == null || inventory != StaticInventoryPass.Inventory)
            {
                inventory = StaticInventoryPass.Inventory;
            } 
        }

        private void UpdateText()
        {
            textSlot.text = typeSlot.ToString();
            string name = "";
            Cell primary = inventory.GetPrimaryWeapon();
            Cell secondary = inventory.GetSecondaryWeapon();
            if (typeSlot == EquipWeaponAt.Primary)
            {
                if (primary != null)
                {
                    name = primary.GetItem().Type.ToString();
                }               
            }
            if (typeSlot == EquipWeaponAt.Secondary)
            {
                if (secondary != null)
                {
                    name = secondary.GetItem().Type.ToString();
                } 
            }
            textName.text = name;
            
        }

        private void UpdateImage()
        {

        }

        void FixedUpdate()
        {
            UpdateInventory();
            if (!(inventory == null))
            {
                UpdateText();
                UpdateImage();
            }
                       
        }
    }
}


