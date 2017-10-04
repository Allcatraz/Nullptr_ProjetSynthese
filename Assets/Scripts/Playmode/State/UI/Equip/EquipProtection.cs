using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class EquipProtection : GameScript
    {
        [SerializeField]
        private ItemType type;

        [SerializeField]
        private Image image;

        private Inventory inventory;

        private void UpdateInventory()
        {
            if (inventory == null || inventory != StaticInventoryPass.Inventory)
            {
                inventory = StaticInventoryPass.Inventory;
            }
        }

        private void UpdateImage()
        {
            Cell vestCell = inventory.GetVest();
            Cell helmetCell = inventory.GetHelmet();
            if (type == ItemType.Vest && vestCell != null)
            {
                Item vest = vestCell.GetItem();
            }
            if (type == ItemType.Helmet && helmetCell != null)
            {
                Item helmet = helmetCell.GetItem();
            }
        }

        private void FixedUpdate()
        {
            UpdateInventory();
            if (!(inventory == null))
            {
                UpdateImage();
            }
            
        }
    }
}

