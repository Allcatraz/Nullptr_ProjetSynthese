using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public enum InventoryOf { Player, Item }
    public enum EquipWeaponAt { Primary, Secondary }

    public delegate void OnInventoryChange();

    public delegate void OnSpawnDroppedItem(Cell itemToSpawn);

    public class Inventory : GameScript
    {
        [SerializeField] private InventoryOf inventoryOf;

        private Cell primaryWeapon;

        private Cell secondaryWeapon;

        private Cell helmet;

        private Cell vest;

        private Cell bag;

        [SerializeField] private float maxWeight = 100;

        private float currentWeight;

        public event OnInventoryChange InventoryChange;

        public event OnSpawnDroppedItem SpawnItem;

        public GameObject parent { get; set; }

        public List<Cell> listInventory { get; private set; }

        private void ChangeMaxWeight(float newWeightToAdd, bool addOrRemove = true)
        {
            if (addOrRemove)
            {
                maxWeight += newWeightToAdd;
            }
            else
            {
                maxWeight -= newWeightToAdd;
            }
        }

        private void RemoveWeight(float weightToRemove)
        {
            currentWeight -= weightToRemove;
            if (currentWeight < 0)
            {
                currentWeight = 0;
            }
        }

        private bool AddWeight(float weightToAdd)
        {
            if (currentWeight + weightToAdd <= maxWeight)
            {
                currentWeight += weightToAdd;
                return true;
            }
            return false;
        }

        public void EquipBag(Cell itemToEquip)
        {
            if (bag != null)
            {
                UnequipBag();
            }
            bag = itemToEquip;
            CheckMultiplePresenceAndRemove(itemToEquip);
            ChangeMaxWeight((itemToEquip.GetItem() as Bag).Capacity);
        }

        public void UnequipBag()
        {
            if (!IsItemPresentInInventory(bag)) listInventory.Add(bag);
            ChangeMaxWeight((bag.GetItem() as Bag).Capacity, false);
            AddWeight(bag.GetItem().GetWeight());
            bag = null;
            NotifyInventoryChange();

        }

        public void ResetInventory()
        {
            currentWeight = 0;
            primaryWeapon = null;
            secondaryWeapon = null;
            helmet = null;
            vest = null;
            listInventory = null;
        }

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

        public void NotifySpawnDroppedItem(Cell itemToSpawn)
        {
            if (SpawnItem != null) SpawnItem(itemToSpawn);
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

        public Cell GetBag()
        {
            return bag;
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

        public void Add(GameObject game, GameObject player)
        {
            CreateListeIsNotExist();
            AddItemCellToInventory(game, player);
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

        public void Drop(Cell itemToDrop)
        {
            itemToDrop.GetItem().Player = parent;
            while (itemToDrop.GetCompteur() > 1)
            {
                RemoveWeight(itemToDrop.GetItem().GetWeight());
                itemToDrop.RemoveOneFromCompteur();
                NotifySpawnDroppedItem(itemToDrop);
                NotifyInventoryChange();
            }
            RemoveWeight(itemToDrop.GetItem().GetWeight());
            listInventory.Remove(itemToDrop);
            NotifySpawnDroppedItem(itemToDrop);
            NotifyInventoryChange();
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
                if ((item.GetItem() as AmmoPack) && (cell.GetItem() as AmmoPack))
                {
                    if ((cell.GetItem() as AmmoPack).AmmoType == (item.GetItem() as AmmoPack).AmmoType)
                    {
                        item.AddCompteur();
                        itemIsPresentInInventory = true;
                        NotifyInventoryChange();
                    }
                }
                else if (item == cell && item.GetItem().Level == cell.GetItem().Level)
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

        private void AddItemCellToInventory(GameObject game, GameObject player = null)
        {
            if (inventoryOf == InventoryOf.Item)
            {
                Cell cell = CreateItemCell(game);
                if (AddWeight(cell.GetItem().GetWeight()))
                {
                    if (!IsItemPresentInInventory(cell)) listInventory.Add(cell);
                    cell.GetItem().Player = player;
                }
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

        public void CheckMultiplePresenceAndRemove(Cell temp)
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
            RemoveWeight(temp.GetItem().GetWeight());
        }

        public static bool operator ==(Inventory one, Inventory two)
        {
            if ((object)one == null && (object)two == null)
            {
                return true;
            }
            if ((object)one == null || (object)two == null)
            {
                return false;
            }
            bool aRetourner = false;
            if (one.listInventory == two.listInventory && one.helmet == two.helmet && one.vest == two.vest && one.primaryWeapon == two.primaryWeapon && one.secondaryWeapon == two.secondaryWeapon)
            {
                aRetourner = true;
            }
            return aRetourner;
        }

        public static bool operator !=(Inventory one, Inventory two)
        {
            return !(one == two);
        }

        public override bool Equals(object obj)
        {
            var inventory = obj as Inventory;
            return inventory != null &&
                   base.Equals(obj) &&
                   inventoryOf == inventory.inventoryOf &&
                   EqualityComparer<Cell>.Default.Equals(primaryWeapon, inventory.primaryWeapon) &&
                   EqualityComparer<Cell>.Default.Equals(secondaryWeapon, inventory.secondaryWeapon) &&
                   EqualityComparer<Cell>.Default.Equals(helmet, inventory.helmet) &&
                   EqualityComparer<Cell>.Default.Equals(vest, inventory.vest) &&
                   EqualityComparer<GameObject>.Default.Equals(parent, inventory.parent) &&
                   EqualityComparer<List<Cell>>.Default.Equals(listInventory, inventory.listInventory);
        }

        public override int GetHashCode()
        {
            var hashCode = 1922053015;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + inventoryOf.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Cell>.Default.GetHashCode(primaryWeapon);
            hashCode = hashCode * -1521134295 + EqualityComparer<Cell>.Default.GetHashCode(secondaryWeapon);
            hashCode = hashCode * -1521134295 + EqualityComparer<Cell>.Default.GetHashCode(helmet);
            hashCode = hashCode * -1521134295 + EqualityComparer<Cell>.Default.GetHashCode(vest);
            hashCode = hashCode * -1521134295 + EqualityComparer<GameObject>.Default.GetHashCode(parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Cell>>.Default.GetHashCode(listInventory);
            return hashCode;
        }
    }
}

