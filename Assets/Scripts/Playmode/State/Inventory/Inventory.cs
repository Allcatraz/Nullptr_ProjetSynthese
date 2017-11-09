using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public enum InventoryOf { Player, Item }
    public enum EquipWeaponAt { Primary, Secondary }

    public delegate void OnInventoryChange();
    public delegate void OnSpawnDroppedItem(ObjectContainedInventory itemToSpawn);

    public class Inventory : GameScript
    {
        [Tooltip("Le type de contenue de l'inventaire.")]
        [SerializeField]
        private InventoryOf inventoryOf;
        [Tooltip("Le poid maximum allant être accepter pour l'inventaire.")]
        [SerializeField]
        private float maxWeight = 100;

        private ObjectContainedInventory primaryWeapon;
        private ObjectContainedInventory secondaryWeapon;
        private ObjectContainedInventory grenade;
        private ObjectContainedInventory helmet;
        private ObjectContainedInventory vest;
        private ObjectContainedInventory bag;
        private float currentWeight;

        public event OnInventoryChange InventoryChange;
        public event OnSpawnDroppedItem SpawnItem;

        public GameObject Parent { get; set; }
        public List<ObjectContainedInventory> ListInventory { get; private set; }

        private void Start()
        {
            Parent = gameObject.transform.parent.gameObject;
        }

        public void ResetInventory()
        {
            currentWeight = 0;
            primaryWeapon = null;
            secondaryWeapon = null;
            helmet = null;
            vest = null;
            ListInventory = null;
            grenade = null;
        }

        public void NotifyInventoryChange()
        {
            if (InventoryChange != null) InventoryChange();
        }

        public void NotifySpawnDroppedItem(ObjectContainedInventory itemToSpawn)
        {
            if (SpawnItem != null) SpawnItem(itemToSpawn);
        }

        public void EquipGrenade(ObjectContainedInventory itemToEquip)
        {
            if (grenade != null)
            {
                UnequipGrenade();
            }
            grenade = itemToEquip;
            CheckMultiplePresenceAndRemove(itemToEquip);
        }

        public void UnequipGrenade()
        {
            if (!IsItemPresentInInventory(grenade)) ListInventory.Add(grenade);
            AddWeight(grenade.GetItem().GetWeight());
            grenade = null;
            NotifyInventoryChange();
        }

        public void EquipBag(ObjectContainedInventory itemToEquip)
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
            if (!IsItemPresentInInventory(bag)) ListInventory.Add(bag);
            ChangeMaxWeight((bag.GetItem() as Bag).Capacity, false);
            AddWeight(bag.GetItem().GetWeight());
            bag = null;
            NotifyInventoryChange();

        }

        public void EquipWeaponAt(EquipWeaponAt selection, ObjectContainedInventory itemToEquip)
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
                if (!IsItemPresentInInventory(primaryWeapon)) ListInventory.Add(primaryWeapon);
                AddWeight(primaryWeapon.GetItem().GetWeight());
                primaryWeapon = null;
                NotifyInventoryChange();
            }
            if (selection == ProjetSynthese.EquipWeaponAt.Secondary && secondaryWeapon != null)
            {
                if (!IsItemPresentInInventory(secondaryWeapon)) ListInventory.Add(secondaryWeapon);
                AddWeight(secondaryWeapon.GetItem().GetWeight());
                secondaryWeapon = null;
                NotifyInventoryChange();
            }
        }

        public void EquipHelmet(ObjectContainedInventory itemToEquip)
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
            if (!IsItemPresentInInventory(helmet)) ListInventory.Add(helmet);
            AddWeight(helmet.GetItem().GetWeight());
            helmet = null;
            NotifyInventoryChange();
        }

        public void EquipVest(ObjectContainedInventory itemToEquip)
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
            if (!IsItemPresentInInventory(vest)) ListInventory.Add(vest);
            AddWeight(vest.GetItem().GetWeight());
            vest = null;
            NotifyInventoryChange();
        }

        public ObjectContainedInventory GetGrenade()
        {
            return grenade;
        }

        public ObjectContainedInventory GetBag()
        {
            return bag;
        }

        public ObjectContainedInventory GetPrimaryWeapon()
        {
            return primaryWeapon;
        }

        public ObjectContainedInventory GetSecondaryWeapon()
        {
            return secondaryWeapon;
        }

        public ObjectContainedInventory GetVest()
        {
            return this.vest;
        }

        public ObjectContainedInventory GetHelmet()
        {
            return this.helmet;
        }

        public void Add(GameObject game)
        {
            CreateListeIfNotExist();
            AddItemCellToInventory(game);
            AddPlayerCellToInventory(game);
        }

        public void Add(GameObject game, GameObject player)
        {
            CreateListeIfNotExist();
            AddItemCellToInventory(game, player);
            AddPlayerCellToInventory(game);
        }

        public void Remove(GameObject game)
        {
            if (inventoryOf == InventoryOf.Item)
            {
                ObjectContainedInventory temp = CreateItemCell(game);
                CheckMultiplePresenceAndRemove(temp);
            }
            if (inventoryOf == InventoryOf.Player)
            {
                ObjectContainedInventory temp = CreatePlayerCell(game);
                CheckMultiplePresenceAndRemove(temp);
            }

        }

        public void Drop(ObjectContainedInventory itemToDrop)
        {
            itemToDrop.GetItem().Player = Parent;
            while (itemToDrop.GetCompteur() > 1)
            {
                RemoveWeight(itemToDrop.GetItem().GetWeight());
                itemToDrop.RemoveOneFromCompteur();
                NotifySpawnDroppedItem(itemToDrop);
                NotifyInventoryChange();
            }
            RemoveWeight(itemToDrop.GetItem().GetWeight());
            ListInventory.Remove(itemToDrop);
            NotifySpawnDroppedItem(itemToDrop);
            NotifyInventoryChange();
        }

        public void DropAll()
        {
            while (ListInventory.Count > 0)
            {
                for (int i = 0; i < this.ListInventory.Count; i++)
                {
                    Drop(ListInventory[i]);
                }
            }   
        }

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

        private void CreateListeIfNotExist()
        {
            if (ListInventory == null)
            {
                ListInventory = new List<ObjectContainedInventory>();
            }
        }

        private void AddPlayerCellToInventory(GameObject game)
        {
            if (inventoryOf == InventoryOf.Player)
            {
                ObjectContainedInventory cell = CreatePlayerCell(game);
                if (!IsItemPresentInInventory(cell)) ListInventory.Add(cell);
                NotifyInventoryChange();
            }
        }

        //Pour AI seulement
        //Calcul la quantité total d'un type objet
        //ex: toutes les helmet peu importe leur force du helmet
        public int GetItemQuantityInInventory(ItemType itemType, AmmoType ammoType)
        {
            Item item = null;
            int quantity = 0;
            if (ListInventory != null)
            {
                foreach (ObjectContainedInventory cell in ListInventory)
                {
                    if (cell != null)
                    {
                        item = cell.GetItem();
                        if (item != null && item.Type == itemType)
                        {
                            if (itemType != ItemType.AmmoPack)
                            {
                                quantity += cell.GetCompteur();
                            }
                            else
                            {
                                AmmoPack ammoPack = item as AmmoPack;
                                if (ammoType == ammoPack.AmmoType)
                                {
                                    quantity += cell.GetCompteur();
                                }
                            }
                        }
                    }
                }
            }
            return quantity;
        }

        private bool IsItemPresentInInventory(ObjectContainedInventory cell)
        {
            bool itemIsPresentInInventory = false;
            foreach (ObjectContainedInventory item in ListInventory)
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

        private static ObjectContainedInventory CreatePlayerCell(GameObject game)
        {
            ObjectContainedInventory cell = new CellPlayer();
            cell.SetItem(game);
            cell.SetImage();
            return cell;
        }

        private void AddItemCellToInventory(GameObject game, GameObject player = null)
        {
            if (inventoryOf == InventoryOf.Item)
            {
                ObjectContainedInventory cell = CreateItemCell(game);
                if (cell.GetItem() != null && AddWeight(cell.GetItem().GetWeight()))
                {
                    if (!IsItemPresentInInventory(cell)) ListInventory.Add(cell);
                    cell.GetItem().Player = player;
                }
                NotifyInventoryChange();
            }
        }

        private static ObjectContainedInventory CreateItemCell(GameObject game)
        {
            ObjectContainedInventory cell = new ItemContainedInventory();
            cell.SetItem(game);
            cell.SetImage();
            return cell;
        }

        public void CheckMultiplePresenceAndRemove(ObjectContainedInventory temp)
        {
            if (temp.GetCompteur() >= 2)
            {
                temp.RemoveOneFromCompteur();
                NotifyInventoryChange();
            }
            else
            {
                ListInventory.Remove(temp);
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
            if (one.ListInventory == two.ListInventory && one.helmet == two.helmet && one.vest == two.vest && one.primaryWeapon == two.primaryWeapon && one.secondaryWeapon == two.secondaryWeapon)
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
                   EqualityComparer<ObjectContainedInventory>.Default.Equals(primaryWeapon, inventory.primaryWeapon) &&
                   EqualityComparer<ObjectContainedInventory>.Default.Equals(secondaryWeapon, inventory.secondaryWeapon) &&
                   EqualityComparer<ObjectContainedInventory>.Default.Equals(helmet, inventory.helmet) &&
                   EqualityComparer<ObjectContainedInventory>.Default.Equals(vest, inventory.vest) &&
                   EqualityComparer<GameObject>.Default.Equals(Parent, inventory.Parent) &&
                   EqualityComparer<List<ObjectContainedInventory>>.Default.Equals(ListInventory, inventory.ListInventory);
        }

        public override int GetHashCode()
        {
            var hashCode = 1922053015;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + inventoryOf.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ObjectContainedInventory>.Default.GetHashCode(primaryWeapon);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObjectContainedInventory>.Default.GetHashCode(secondaryWeapon);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObjectContainedInventory>.Default.GetHashCode(helmet);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObjectContainedInventory>.Default.GetHashCode(vest);
            hashCode = hashCode * -1521134295 + EqualityComparer<GameObject>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<ObjectContainedInventory>>.Default.GetHashCode(ListInventory);
            return hashCode;
        }
    }
}