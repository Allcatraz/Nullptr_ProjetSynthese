using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class InventoryController : GameScript {

        [SerializeField] private GameObject cellObjectPrefab;
        [SerializeField] private Transform gridInventoryPlayer;
        [SerializeField] private Transform gridEquippedByPlayer;

        private bool abonner = false;
        private Inventory inventory;

        private void ClearGridInventoryPlayer()
        {
            foreach (Transform child in gridInventoryPlayer)
            {
                Destroy(child.gameObject);
            }
        }

        private void Inventory_InventoryChange()
        {
            UpdateInventory();
            CreateCellsForInventoryPlayer();
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
            if (inventory != StaticInventoryPass.Inventory)
            {
                UpdateInventory();
                CreateCellsForInventoryPlayer();
                DisconnectFromEvent();
            }
            else
            {
                ConnectToEvent();

            }
        }

        private void DisconnectFromEvent()
        {
            if (abonner == true)
            {
                inventory.InventoryChange -= Inventory_InventoryChange;
                abonner = false;
            }
        }

        private void ConnectToEvent()
        {
            if (abonner == false)
            {
                inventory.InventoryChange += Inventory_InventoryChange;
                abonner = true;
            }
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
                    cellObject.GetComponentInChildren<CellObject>().inventory = this.inventory;
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
