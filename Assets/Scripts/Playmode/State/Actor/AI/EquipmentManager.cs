
using UnityEngine;
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
            if (!Actor.Brain.HasVestEquipped)
            {
                Item item = null;
                if (Actor.AIInventory.ListInventory != null)
                {
                    foreach (ObjectContainedInventory cell in Actor.AIInventory.ListInventory)
                    {
                        item = cell.GetItem();
                        if (item.Type == ItemType.Vest)
                        {
                            Actor.AIInventory.EquipVest(cell);
                            Actor.Brain.HasVestEquipped = true;
                            break;
                        }
                    }
                }
            }
        }
        public void SelectHelmet()
        {
            if (!Actor.Brain.HasHelmetEquipped)
            {
                Item item = null;
                if (Actor.AIInventory.ListInventory != null)
                {
                    foreach (ObjectContainedInventory cell in Actor.AIInventory.ListInventory)
                    {
                        item = cell.GetItem();
                        if (item.Type == ItemType.Helmet)
                        {
                            Actor.AIInventory.EquipHelmet(cell);
                            Actor.Brain.HasHelmetEquipped = true;
                            break;
                        }
                    }
                }
            }
        }
        public bool IsInventoryEmpty()
        {
            if (Actor.AIInventory.ListInventory != null && (Actor.AIInventory.ListInventory.Count > 0)
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