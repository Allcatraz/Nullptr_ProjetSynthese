using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public enum InventoryOf {Player, Item}

    [AddComponentMenu("Game/State/Health")]
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
                listInventory.Add(cell);
            }
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
                listInventory.Add(cell);
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
                listInventory.Remove(CreateItemCell(game));
            }
            if (inventoryOf == InventoryOf.Player)
            {
                listInventory.Remove(CreatePlayerCell(game));
            }
            
        }
    }
}

