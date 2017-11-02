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
                        || item.Type == ItemType.Saiga12
                        || item.Type == ItemType.Grenade)
                        {
                            Actor.AIInventory.EquipWeaponAt(EquipWeaponAt.Primary, cell);
                            Weapon weapon = (Weapon)Actor.AIInventory.GetPrimaryWeapon().GetItem();
                            if (weapon.Reload())
                            {
                                Actor.Brain.HasPrimaryWeaponEquipped = true;
                                break;
                            }
                            else
                            {
                                Actor.AIInventory.UnequipWeaponAt(EquipWeaponAt.Primary);
                            }
                        }
                    }
                }

            }
        }
        public void SelectVest()
        {
            if (Actor.Brain.HasVestEquipped)
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
                }
            }
        }
        public void SelectHelmet()
        {
            if (Actor.Brain.HasHelmetEquipped)
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
                }
            }
        }

        public void SelectBag()
        {
            if (Actor.Brain.HasBagEquipped)
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
                }
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