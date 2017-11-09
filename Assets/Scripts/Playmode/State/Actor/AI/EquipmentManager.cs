namespace ProjetSynthese
{
    public class EquipmentManager
    {
        private readonly ActorAI Actor;

        public EquipmentManager(ActorAI actor)
        {
            this.Actor = actor;
        }

        public void SelectWeapon()
        {
            if (!Actor.Brain.HasPrimaryWeaponEquipped)
            {
                Item item = null;
                if (Actor.AIInventory.ListInventory != null)
                {
                    foreach (ObjectContainedInventory cell in Actor.AIInventory.ListInventory)
                    {
                        item = cell.GetItem();
                        if (item.Type == ItemType.AWM
                        || item.Type == ItemType.M16A4
                        || item.Type == ItemType.M1911
                        || item.Type == ItemType.Saiga12)
                        {
                            Actor.AIInventory.EquipWeaponAt(EquipWeaponAt.Primary, cell);
                            Weapon weapon = (Weapon)Actor.AIInventory.GetPrimaryWeapon().GetItem();
                           
                            //Actor.Brain.GetAmmoPackStorageRatio(ammoPack.AmmoType)
                            if (weapon.Reload())
                            {
                                Actor.Brain.HasPrimaryWeaponEquipped = true;
                                break;
                            }
                            else
                            {
                                Actor.AIInventory.UnequipWeaponAt(EquipWeaponAt.Primary);
                                Actor.Brain.HasPrimaryWeaponEquipped = false;
                            }
                        }
                    }
                }

            }
        }
        public void SelectVest()
        {
            if (Actor.Brain.HasVestEquipped && Actor.Brain.InventoryBestVest != null)
            {
                if(((Vest)Actor.Brain.InventoryBestVest.GetItem()).ProtectionValue >
               ((Vest)Actor.AIInventory.GetVest().GetItem()).ProtectionValue)
                {
                    Actor.AIInventory.EquipVest(Actor.Brain.InventoryBestVest);
                }
            }
            else
            {
                if (Actor.Brain.InventoryBestVest != null)
                {
                   Actor.AIInventory.EquipVest(Actor.Brain.InventoryBestVest);
                   Actor.Brain.HasVestEquipped = true;
                   Actor.Brain.InventoryBestVest = null;
                }
            }
        }
        public void SelectHelmet()
        {
            if (Actor.Brain.HasHelmetEquipped && Actor.Brain.InventoryBestHelmet!=null)
            {
                if (((Helmet)Actor.Brain.InventoryBestHelmet.GetItem()).ProtectionValue >
               ((Helmet)Actor.AIInventory.GetHelmet().GetItem()).ProtectionValue)
                {
                    Actor.AIInventory.EquipHelmet(Actor.Brain.InventoryBestHelmet);
                }
            }
            else
            {
                if (Actor.Brain.InventoryBestHelmet != null)
                {
                    Actor.AIInventory.EquipHelmet(Actor.Brain.InventoryBestHelmet);
                    Actor.Brain.HasHelmetEquipped = true;
                    Actor.Brain.InventoryBestHelmet = null;
                }
            }
        }

        public void SelectBag()
        {
            if (Actor.Brain.HasBagEquipped && Actor.Brain.InventoryBestBag != null)
            {
                if (((Bag)Actor.Brain.InventoryBestBag.GetItem()).Capacity >
               ((Bag)Actor.AIInventory.GetBag().GetItem()).Capacity)
                {
                    Actor.AIInventory.EquipBag(Actor.Brain.InventoryBestBag);
                }
            }
            else
            {
                if (Actor.Brain.InventoryBestBag != null)
                {
                    Actor.AIInventory.EquipBag(Actor.Brain.InventoryBestBag);
                    Actor.Brain.HasBagEquipped = true;
                    Actor.Brain.InventoryBestBag = null;
                }
            }
        }
        public void UseBoost()
        {
            if (Actor.Brain.InventoryBestBoost != null)
            {
                Actor.Brain.InventoryBestBoost.GetItem().Player = Actor.gameObject;

                (Actor.Brain.InventoryBestBoost.GetItem() as Usable).Use();
                Actor.AIInventory.CheckMultiplePresenceAndRemove(Actor.Brain.InventoryBestBoost);
                Actor.Brain.InventoryBestBoost = null;
            }
            
        }
        public void UseHeal()
        {
            if (Actor.Brain.InventoryBestHeal != null)
            {
                Actor.Brain.InventoryBestHeal.GetItem().Player = Actor.gameObject;

                (Actor.Brain.InventoryBestHeal.GetItem() as Usable).Use();
                Actor.AIInventory.CheckMultiplePresenceAndRemove(Actor.Brain.InventoryBestHeal);
                Actor.Brain.InventoryBestHeal = null;
            }
            
        }
        public bool IsInventoryEmpty()
        {
            if (Actor.AIInventory.ListInventory != null && Actor.AIInventory.ListInventory.Count > 0)
            {
                 return false;
            }
            else
            {
                return true;
            }
        }
    }
}