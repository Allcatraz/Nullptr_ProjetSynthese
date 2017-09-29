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
        private Image iamge;

        private Inventory inventory;

        private void UpdateInventory()
        {
            if (inventory != StaticInventoryPass.Inventory)
            {
                inventory = StaticInventoryPass.Inventory;
            }
        }

        private void UpdateImage()
        {
            if (type == ItemType.Vest)
            {
                Item vest = inventory.GetVest().GetItem();
            }
            if (type == ItemType.Helmet)
            {
                Item helmet = inventory.GetHelmet().GetItem();
            }
        }

        void Update()
        {
            UpdateInventory();
            UpdateImage();
        }
    }
}

