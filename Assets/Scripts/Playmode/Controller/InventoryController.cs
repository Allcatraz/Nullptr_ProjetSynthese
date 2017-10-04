
using UnityEngine;
using System.Collections.Generic;

namespace ProjetSynthese
{
    public class InventoryController : GameScript
    {
        [SerializeField] private Transform gridInventoryPlayer;
        [SerializeField] private Transform gridEquippedByPlayer;
        [SerializeField] private Transform gridProtectionPlayer;
        [SerializeField] private Transform gridNerbyItem;
        [SerializeField] private GameObject cellEquippedWeaponPrefabs;
        [SerializeField] private GameObject cellProtectionItemPrefabs;
        [SerializeField] private GameObject cellObjectPrefab;
        [SerializeField] private Inventory inventoryGround;
        [SerializeField] private ItemSensor sensorItem;
        private bool abonner = false;
        private Inventory inventory;

        public void CreateCellsForInventoryPlayer()
        {
            ClearGrid(gridInventoryPlayer);
            foreach (Cell item in inventory.listInventory)
            {
                GameObject cellObject = Instantiate(cellObjectPrefab);
                cellObject.transform.SetParent(gridInventoryPlayer, false);
                cellObject.GetComponentInChildren<CellObject>().inventory = this.inventory;
                cellObject.GetComponentInChildren<CellObject>().InstantiateFromCell(item);
            }
        }

        public void CreateCellsForWeaponByPlayer()
        {
            ClearGrid(gridEquippedByPlayer);
            if (inventory.GetPrimaryWeapon() != null)
            {
                GameObject cellWeaponTemp1 = Instantiate(cellEquippedWeaponPrefabs);
                cellWeaponTemp1.transform.SetParent(gridEquippedByPlayer, false);
                cellWeaponTemp1.GetComponentInChildren<CellObject>().inventory = this.inventory;
                cellWeaponTemp1.GetComponentInChildren<CellObject>().InstantiateFromCell(inventory.GetPrimaryWeapon());
                cellWeaponTemp1.GetComponentInChildren<CellObject>().equipAt = EquipWeaponAt.Primary;
            }
            if (inventory.GetSecondaryWeapon() != null)
            {
                GameObject cellWeaponTemp2 = Instantiate(cellEquippedWeaponPrefabs);
                cellWeaponTemp2.transform.SetParent(gridEquippedByPlayer, false);
                cellWeaponTemp2.GetComponentInChildren<CellObject>().inventory = this.inventory;
                cellWeaponTemp2.GetComponentInChildren<CellObject>().InstantiateFromCell(inventory.GetSecondaryWeapon());
                cellWeaponTemp2.GetComponentInChildren<CellObject>().equipAt = EquipWeaponAt.Secondary;
            }
        }

        public void CreateCellsForProtectionPlayer()
        {
            ClearGrid(gridProtectionPlayer);
            if (inventory.GetVest() != null)
            {
                GameObject cellProtectionTemp1 = Instantiate(cellProtectionItemPrefabs);
                cellProtectionTemp1.transform.SetParent(gridProtectionPlayer, false);
                cellProtectionTemp1.GetComponentInChildren<CellObject>().inventory = this.inventory;
                cellProtectionTemp1.GetComponentInChildren<CellObject>().InstantiateFromCell(inventory.GetVest());
            }

            if (inventory.GetHelmet() != null)
            {
                GameObject cellProtectionTemp2 = Instantiate(cellProtectionItemPrefabs);
                cellProtectionTemp2.transform.SetParent(gridProtectionPlayer, false);
                cellProtectionTemp2.GetComponentInChildren<CellObject>().inventory = this.inventory;
                cellProtectionTemp2.GetComponentInChildren<CellObject>().InstantiateFromCell(inventory.GetHelmet());
            }
        }

        public void CreateCellsForNearbyItem()
        {
            ClearGrid(gridNerbyItem);
            CreateInventoryGround();
            if (inventoryGround.listInventory != null)
            {
                foreach (Cell item in inventoryGround.listInventory)
                {
                    GameObject cellObject = Instantiate(cellObjectPrefab);
                    cellObject.transform.SetParent(gridNerbyItem, false);
                    cellObject.GetComponentInChildren<CellObject>().inventory = inventoryGround;
                    cellObject.GetComponentInChildren<CellObject>().InstantiateFromCell(item);
                }
            }
        }

        private void CreateInventoryGround()
        {
            inventoryGround.ResetInventory();
            List<GameObject> listTemp = sensorItem.GetAllItems();
            foreach (GameObject item in listTemp)
            {
                inventoryGround.Add(item);
            }
        }

        private void ClearGrid(Transform grid)
        {
            foreach (Transform child in grid)
            {
                Destroy(child.gameObject);
            }
        }

        private void Inventory_InventoryChange()
        {
            UpdateInventory();
            CreateCellsForInventoryPlayer();
            CreateCellsForWeaponByPlayer();
            CreateCellsForProtectionPlayer();
            CreateCellsForNearbyItem();
        }

        private void FixedUpdate()
        {
            if (inventory == null || inventory != StaticInventoryPass.Inventory)
            {
                Inventory_InventoryChange();
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

        private void UpdateInventory()
        {
            inventory = StaticInventoryPass.Inventory;
        }
    }
}
