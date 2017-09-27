using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class InventoryController : GameScript {

        [SerializeField]
        private GameObject cellObjectPrefab;

        private Inventory inven;

        public void OpenInventory()
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
                    cellObject.transform.SetParent(this.gameObject.transform.GetChild(0).transform.GetChild(0), false);
                    cellObject.GetComponentInChildren<CellObject>().InstantiateFromCell(item);
                }
            }
        }
    }
}
