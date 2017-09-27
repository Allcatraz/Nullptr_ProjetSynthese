using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class InventoryController : GameScript {

        [SerializeField]
        private GameObject cellObjectPrefab;

        private Inventory inven;

	    private void Start ()
        {
            inven = StaticInventoryPass.inventory;
            InstantiateCellObjectFromCell();
	    }

        private void InstantiateCellObjectFromCell()
        {
            if (inven != null)
            {
                foreach (Cell item in inven.listInventory)
                {
                    GameObject cellObject = Instantiate(cellObjectPrefab);
                    cellObject.transform.SetParent(this.gameObject.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0), false);
                    cellObject.GetComponentInChildren<CellObject>().InstantiateFromCell(item);
                }
            }  
        }
    }
}
