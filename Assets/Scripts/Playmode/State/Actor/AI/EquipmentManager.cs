using UnityEngine;
namespace ProjetSynthese
{
    public class EquipmentManager
    {
        private readonly ActorAI Actor;

        Weapon currentWeapon = null;

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
                        if (item.Type == ItemType.M110
                        || item.Type == ItemType.M16A4
                        || item.Type == ItemType.MP5
                        || item.Type == ItemType.BenelliM4)
                        {
                            Weapon weapon = (Weapon)cell.GetItem();
                            AmmoType ammoType = weapon.WeaponAmmoType;
                            
                            if (weapon.MagazineAmount > 0 || weapon.Reload(Actor.AIInventory))
                            {
                                Actor.AIInventory.EquipWeaponAt(EquipWeaponAt.Primary, cell);
                                Actor.Brain.HasPrimaryWeaponEquipped = true;
                                currentWeapon = weapon;
                                break;
                            }
                            else
                            {
                                Actor.AIInventory.UnequipWeaponAt(EquipWeaponAt.Primary);
                                Actor.Brain.HasPrimaryWeaponEquipped = false;
                                currentWeapon = null;
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

                ((Usable) Actor.Brain.InventoryBestBoost.GetItem()).Use();
                Actor.AIInventory.CheckMultiplePresenceAndRemove(Actor.Brain.InventoryBestBoost);
                Actor.Brain.InventoryBestBoost = null;
            }
            
        }
        public void UseHeal()
        {
            if (Actor.Brain.InventoryBestHeal != null)
            {
                Actor.Brain.InventoryBestHeal.GetItem().Player = Actor.gameObject;

                ((Usable) Actor.Brain.InventoryBestHeal.GetItem()).Use();
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
            return true;
        }
        private bool CheckWeaponAmmunitionStatus()
        {
            return currentWeapon != null && (currentWeapon.MagazineAmount > 0 || currentWeapon.Reload(Actor.AIInventory));
        }

        public bool WeaponReadyToUse()
        {
            bool weaponCanBeUsed = true;
            if (CheckWeaponAmmunitionStatus())
            {
                Vector3 target = Actor.transform.position;
                target.y = 0.0f;
                currentWeapon.transform.position = target;
                currentWeapon.transform.rotation = Actor.transform.rotation;
            }
            else
            {
                Actor.AIInventory.UnequipWeaponAt(EquipWeaponAt.Primary);
                currentWeapon = null;
                Actor.Brain.HasPrimaryWeaponEquipped = false;
                weaponCanBeUsed = false;
            }
            return weaponCanBeUsed;
        }

        public Weapon GetWeapon()
        {
            return currentWeapon;
        }
    }
}