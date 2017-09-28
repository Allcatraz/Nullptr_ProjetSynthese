using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class InventoryController : GameScript {

        [SerializeField] private GameObject cellObjectPrefab;
        [SerializeField] private Transform gridInventoryPlayer;
        [SerializeField] private Transform gridEquippedByPlayer;

        private Inventory inventory;

        private void ClearGridInventoryPlayer()
        {
            foreach (Transform child in gridInventoryPlayer)
            {
                Destroy(child.gameObject);
            }
        }

        private void ClearGridEquippedByPlayer()
        {
            foreach (Transform child in gridEquippedByPlayer)
            {
                Destroy(child.gameObject);
            }
        }

        private void FixedUpdate()
        {
            UpdateInventory();
            CreateCellsForInventoryPlayer();
        }

        public void CreateCellsForInventoryPlayer()
        {
            ClearGridInventoryPlayer();

            if (inventory != null)
            {
                foreach (Cell item in inventory.listInventory)
                {
                    GameObject cellObject = Instantiate(cellObjectPrefab);
                    cellObject.transform.SetParent(gridInventoryPlayer, false);
                    cellObject.GetComponentInChildren<CellObject>().InstantiateFromCell(item);
                }
            }
        }

        public void CreateCellsForEquippedByPlayer()
        {
            ClearGridEquippedByPlayer();
            if (inventory != null)
            {

            }
            
        }

        private void UpdateInventory()
        {
            inventory = StaticInventoryPass.Inventory;
        }
    }
}
