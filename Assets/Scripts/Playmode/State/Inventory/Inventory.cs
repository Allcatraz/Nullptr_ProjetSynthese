using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public enum InventoryOf {Player, Item}

    [AddComponentMenu("Game/State/Inventory/Inventory")]
    public class Inventory : GameScript
    {

        [SerializeField]
        private InventoryOf inventoryOf;

        private List<Cell> listInventory;

        public void Add(GameObject game)
        {
            AddItemCellToInventory(game);
            AddPlayerCellToInventory(game);
        }

        private void AddPlayerCellToInventory(GameObject game)
        {
            if (inventoryOf == InventoryOf.Player)
            {
                Cell cell = CreatePlayerCell(game);
                if (IsItemPresentInInventory(cell)) listInventory.Add(cell);

            }
        }

        private bool IsItemPresentInInventory(Cell cell)
        {
            bool itemIsPresentInInventory = false;
            foreach (Cell item in listInventory)
            {
                if (item == cell)
                {
                    item.AddCompteur();
                    itemIsPresentInInventory = true;
                    break;
                }
            }
            return itemIsPresentInInventory;
        }

        private static Cell CreatePlayerCell(GameObject game)
        {
            Cell cell = new CellPlayer();
            cell.SetItem(game);
            cell.SetImage();
            return cell;
        }

        private void AddItemCellToInventory(GameObject game)
        {
            if (inventoryOf == InventoryOf.Item)
            {
                Cell cell = CreateItemCell(game);
                if (IsItemPresentInInventory(cell)) listInventory.Add(cell);
            }
        }

        private static Cell CreateItemCell(GameObject game)
        {
            Cell cell = new CellItem();
            cell.SetItem(game);
            cell.SetImage();
            return cell;
        }

        public void Remove(GameObject game)
        {
            if (inventoryOf == InventoryOf.Item)
            {
                Cell temp = CreateItemCell(game);
                CheckMultiplePresenceAndRemove(temp);
            }
            if (inventoryOf == InventoryOf.Player)
            {
                Cell temp = CreatePlayerCell(game);
                CheckMultiplePresenceAndRemove(temp);
            }
            
        }

        private void CheckMultiplePresenceAndRemove(Cell temp)
        {
            if (temp.GetCompteur() >= 2)
            {
                temp.RemoveOneFromCompteur();
            }
            else
            {
                listInventory.Remove(temp);
            }
        }
    }
}

