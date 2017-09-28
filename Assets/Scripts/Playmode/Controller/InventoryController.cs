using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class InventoryController : GameScript {

        [SerializeField] private GameObject cellObjectPrefab;
        [SerializeField] private Transform grid;

        private Inventory inven;

        private void Clear()
        {
            foreach (Transform child in grid)
            {
                Destroy(child.gameObject);
            }
        }

        private void FixedUpdate()
        {
            InstantiateCellObjectFromCell();
        }

        public void InstantiateCellObjectFromCell()
        {
            Clear();
            inven = StaticInventoryPass.Inventory;

            if (inven != null)
            {
                foreach (Cell item in inven.listInventory)
                {
                    GameObject cellObject = Instantiate(cellObjectPrefab);
                    cellObject.transform.SetParent(grid, false);
                    cellObject.GetComponentInChildren<CellObject>().InstantiateFromCell(item);
                }
            }
        }
    }
}
