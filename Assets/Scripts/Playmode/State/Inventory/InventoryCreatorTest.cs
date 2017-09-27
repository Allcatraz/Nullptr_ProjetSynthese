using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class InventoryCreatorTest : GameScript
    {
        [SerializeField]
        private GameObject inventory;

        [SerializeField]
        private GameObject[] itemTemp;

        private Inventory inven;

        /// <summary>
        /// Creation inventaire predeterminer pour test
        /// </summary>
        private void Start()
        {
            inven = inventory.GetComponent<Inventory>();
            GameObject temp1 = Instantiate(itemTemp[0]);
            temp1.GetComponent<Weapon>().Type = ItemType.M16A4;
            GameObject temp3 = Instantiate(itemTemp[0]);
            temp3.GetComponent<Weapon>().Type = ItemType.M1911;
            GameObject temp2 = Instantiate(itemTemp[0]);
            temp2.GetComponent<Weapon>().Type = ItemType.Saiga12;
            inven.Add(temp1);
            inven.Add(temp2);
            inven.Add(temp3);
            StaticInventoryPass.inventory = inven;

        }

    }
}

