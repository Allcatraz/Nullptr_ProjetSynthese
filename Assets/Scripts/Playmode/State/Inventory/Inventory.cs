using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public enum InventoryOf { Player, Item }
    public enum EquipWeaponAt { Primary, Secondary}

    public delegate void OnInventoryChange();

    [AddComponentMenu("Game/State/Inventory/Inventory")]
    public class Inventory : GameScript
    {
        [SerializeField] private InventoryOf inventoryOf;

        private Cell primaryWeapon;

        private Cell secondaryWeapon;

        private Cell helmet;

        private Cell vest;

        public event OnInventoryChange InventoryChange;

        public GameObject parent { get; set; }

        public List<Cell> listInventory { get; private set; }

        public void EquipWeaponAt(EquipWeaponAt selection, Cell itemToEquip)
        {
            if (selection == ProjetSynthese.EquipWeaponAt.Primary)
            {
                if (primaryWeapon != null)
                {
                    UnequipWeaponAt(ProjetSynthese.EquipWeaponAt.Primary);
                }
                primaryWeapon = itemToEquip;
                CheckMultiplePresenceAndRemove(itemToEquip);
            }
            if (selection == ProjetSynthese.EquipWeaponAt.Secondary)
            {
                if (secondaryWeapon != null)
                {
                    UnequipWeaponAt(ProjetSynthese.EquipWeaponAt.Secondary);
                }
                secondaryWeapon = itemToEquip;
                CheckMultiplePresenceAndRemove(itemToEquip);
            }
        }
        
        public void UnequipWeaponAt(EquipWeaponAt selection)
        {
            if (selection == ProjetSynthese.EquipWeaponAt.Primary && primaryWeapon != null)
            {
                if (!IsItemPresentInInventory(primaryWeapon)) listInventory.Add(primaryWeapon);
                primaryWeapon = null;
                NotifyInventoryChange();
            }
            if (selection == ProjetSynthese.EquipWeaponAt.Secondary && secondaryWeapon != null)
            {
                if (!IsItemPresentInInventory(secondaryWeapon)) listInventory.Add(secondaryWeapon);
                secondaryWeapon = null;
                NotifyInventoryChange();
            }
        }

        public void NotifyInventoryChange()
        {
            if (InventoryChange != null) InventoryChange();
        }

        public void EquipHelmet(Cell itemToEquip)
        {
            if (helmet != null)
            {
                UnequipHelmet();
            }
            helmet = itemToEquip;
            CheckMultiplePresenceAndRemove(itemToEquip);
        }

        public void UnequipHelmet()
        {
            if (!IsItemPresentInInventory(helmet)) listInventory.Add(helmet);
            helmet = null;
            NotifyInventoryChange();
        }

        public void EquipVest(Cell itemToEquip)
        {
            if (vest != null)
            {
                UnequipVest();
            }
            vest = itemToEquip;
            CheckMultiplePresenceAndRemove(itemToEquip);
        }

        public void UnequipVest()
        {
            if (!IsItemPresentInInventory(vest)) listInventory.Add(vest);
            vest = null;
            NotifyInventoryChange();
        }

        public Cell GetPrimaryWeapon()
        {
            return primaryWeapon;
        }

        public Cell GetSecondaryWeapon()
        {
            return secondaryWeapon;
        }

        public Cell GetVest()
        {
            return this.vest;
        }

        public Cell GetHelmet()
        {
            return this.helmet;
        }

        public void Add(GameObject game)
        {
            CreateListeIsNotExist();
            AddItemCellToInventory(game);
            AddPlayerCellToInventory(game);
        }

        public void Add(Item item)
        {
            CreateListeIsNotExist();
            Cell cell = new CellItem(item);
            if (!IsItemPresentInInventory(cell)) listInventory.Add(cell);
            NotifyInventoryChange();
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

        private void Start()
        {
            parent = this.gameObject.transform.parent.gameObject;
            CreateListeIsNotExist();
        }

        private void CreateListeIsNotExist()
        {
            if (listInventory == null)
            {
                listInventory = new List<Cell>();
            }
        }

        private void AddPlayerCellToInventory(GameObject game)
        {
            if (inventoryOf == InventoryOf.Player)
            {
                Cell cell = CreatePlayerCell(game);
                if (!IsItemPresentInInventory(cell)) listInventory.Add(cell);
                NotifyInventoryChange();
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
                    NotifyInventoryChange();
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
                if (!IsItemPresentInInventory(cell)) listInventory.Add(cell);
                NotifyInventoryChange();
            }
        }

        private static Cell CreateItemCell(GameObject game)
        {
            Cell cell = new CellItem();
            cell.SetItem(game);
            cell.SetImage();
            return cell;
        }

        private void CheckMultiplePresenceAndRemove(Cell temp)
        {
            if (temp.GetCompteur() >= 2)
            {
                temp.RemoveOneFromCompteur();
                NotifyInventoryChange();
            }
            else
            {
                listInventory.Remove(temp);
                NotifyInventoryChange();
            }
        }

        public static bool operator ==(Inventory one, Inventory two)
        {
            if ((object)one == null || (object)two == null)
                return false;
            bool aRetourner = false;
            if (one.listInventory == two.listInventory && one.helmet == two.helmet && one.vest == two.vest && one.primaryWeapon == two.primaryWeapon && one.secondaryWeapon == two.secondaryWeapon)
            {
                aRetourner = true;
            }
            return aRetourner;
        }

        public static bool operator !=(Inventory one,   Inventory two)
        {
            return !(one == two);
        }
    }
}

