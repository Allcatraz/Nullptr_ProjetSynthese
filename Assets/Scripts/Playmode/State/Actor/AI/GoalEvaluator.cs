
using UnityEngine;

namespace ProjetSynthese
{
    public class GoalEvaluator
    {

        private readonly ActorAI actor;

        private const float NoWeaponTrackGoalLevel = 0.0f;
        private const float EmptyInventoryLootGoalLevel = 1.0f;
        private const float TrackGoalFactor = 1.0f;
        private const float LootGoalFactor = 1.0f;
        private const float VestGoalFactor = 1.0f;
        private const float HelmetGoalFactor = 1.0f;
        private const float HealGoalFactor = 1.0f;
        private const float BoostGoalFactor = 1.0f;
        private const float BagGoalFactor = 1.0f;
        private const float WeaponGoalFactor = 1.0f;
        private const float AmmoPackGoalFactor = 1.0f;

        private const float ErrorGoalTolerance = 0.0001f;

        float[] inventoryValues = null;
        float[] foundItemsValues = null;

        public GoalEvaluator(ActorAI actor)
        {
            this.actor = actor;
        }

        private void EvaluateInventoryValue()
        {
            Item item = null;
            inventoryValues = new float[actor.AIInventory.ListInventory.Count];
            float distanceToItem = 1.0f;
            int i = 0;
            if (actor.AIInventory.ListInventory != null)
            {
                foreach (ObjectContainedInventory cell in actor.AIInventory.ListInventory)
                {
                    item = cell.GetItem();
                    inventoryValues[i] = 0.0f;
                    switch (item.Type)
                    {
                        case ItemType.M16A4:
                        case ItemType.AWM:
                        case ItemType.Saiga12:
                        case ItemType.M1911:
                            inventoryValues[i] = EvaluateWeaponValue(distanceToItem, item);
                            break;
                        case ItemType.Grenade:
                            inventoryValues[i] = EvaluateGrenadeValue(distanceToItem, item);
                            break;
                        case ItemType.Helmet:
                            inventoryValues[i] = EvaluateHelmetValue(distanceToItem, item);
                            break;
                        case ItemType.Vest:
                            inventoryValues[i] = EvaluateVestValue(distanceToItem, item);
                            break;
                        case ItemType.Bag:
                            inventoryValues[i] = EvaluateBagValue(distanceToItem, item);
                            break;
                        case ItemType.Heal:
                            inventoryValues[i] = EvaluateHealValue(distanceToItem, item);
                            break;
                        case ItemType.Boost:
                            inventoryValues[i] = EvaluateBoostValue(distanceToItem, item);
                            break;
                        case ItemType.AmmoPack:
                            inventoryValues[i] = EvaluateAmmoPackValue(distanceToItem, item);
                            break;
                        default:
                            break;
                    }
                    i++;
                }
            }
        }

        private void EvaluateFoundItemsValues(Item[] items)
        {
            if (items != null)
            {
                Item item = null;
                foundItemsValues = new float[items.Length];
                float distanceToItem = 1.0f;
                for (int i = 0; i < items.Length; i++)
                {
                    item = items[i];
                    foundItemsValues[i] = 0.0f;
                    distanceToItem = Vector3.Distance(actor.transform.position, item.transform.position);
                    switch (item.Type)
                    {
                        case ItemType.M16A4:
                        case ItemType.AWM:
                        case ItemType.Saiga12:
                        case ItemType.M1911:
                            foundItemsValues[i] = EvaluateWeaponValue(distanceToItem, item);
                            break;
                        case ItemType.Grenade:
                            foundItemsValues[i] = EvaluateGrenadeValue(distanceToItem, item);
                            break;
                        case ItemType.Helmet:
                            foundItemsValues[i] = EvaluateHelmetValue(distanceToItem, item);
                            break;
                        case ItemType.Vest:
                            foundItemsValues[i] = EvaluateVestValue(distanceToItem, item);
                            break;
                        case ItemType.Bag:
                            foundItemsValues[i] = EvaluateBagValue(distanceToItem, item);
                            break;
                        case ItemType.Heal:
                            foundItemsValues[i] = EvaluateHealValue(distanceToItem, item);
                            break;
                        case ItemType.Boost:
                            foundItemsValues[i] = EvaluateBoostValue(distanceToItem, item);
                            break;
                        case ItemType.AmmoPack:
                            foundItemsValues[i] = EvaluateAmmoPackValue(distanceToItem, item);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public float EvaluateItemValue(Item item, float distanceToItem)
        {
            float itemValue = 0.0f;
            if (item != null && distanceToItem >= 0.0f)
            {
                switch (item.Type)
                {
                    case ItemType.M16A4:
                    case ItemType.AWM:
                    case ItemType.Saiga12:
                    case ItemType.M1911:
                        itemValue = EvaluateWeaponValue(distanceToItem, item);
                        break;
                    case ItemType.Grenade:
                        itemValue = EvaluateGrenadeValue(distanceToItem, item);
                        break;
                    case ItemType.Helmet:
                        itemValue = EvaluateHelmetValue(distanceToItem, item);
                        break;
                    case ItemType.Vest:
                        itemValue = EvaluateVestValue(distanceToItem, item);
                        break;
                    case ItemType.Bag:
                        itemValue = EvaluateBagValue(distanceToItem, item);
                        break;
                    case ItemType.Heal:
                        itemValue = EvaluateHealValue(distanceToItem, item);
                        break;
                    case ItemType.Boost:
                        itemValue = EvaluateBoostValue(distanceToItem, item);
                        break;
                    case ItemType.AmmoPack:
                        itemValue = EvaluateAmmoPackValue(distanceToItem, item);
                        break;
                    default:
                        break;
                }
            }
            return itemValue;
        }
        public float EvaluateLootGoal()
        {
            
            float lootGoalLevel = 0.0f;
  
            if (actor.EquipmentManager.IsInventoryEmpty())
            {
                lootGoalLevel += EmptyInventoryLootGoalLevel;
            }
            else
            {
                Item item= actor.Brain.ItemInPerceptionRange;
                float distanceToItem = Vector3.Distance(actor.transform.position, item.transform.position);
                if (actor.Brain.ItemInPerceptionRange != null)
                {
                    lootGoalLevel += EvaluateItemValue(item, distanceToItem);
                }
            }

            return lootGoalLevel;
        }

        public float EvaluateLootHeapGoal(Item[] items)
        {

            float lootHeapGoalLevel = 0.0f;

            if (actor.EquipmentManager.IsInventoryEmpty())
            {
                lootHeapGoalLevel += EmptyInventoryLootGoalLevel;
            }
            else
            {
                if (items != null && foundItemsValues.Length > 0)
                {
                    EvaluateFoundItemsValues(items);
                    for (int i = 0; i < foundItemsValues.Length; i++)
                    {
                        lootHeapGoalLevel += foundItemsValues[i];
                    }
                    lootHeapGoalLevel /= foundItemsValues.Length;
                }
            }

            return lootHeapGoalLevel;
        }
        public float EvaluateTrackGoal()
        {
            float trackGoalLevel = 0.0f;

            //track = ka *health*weaponstrength(vestpower*helmetPower
            //ajoute ammunition level 
            if (!actor.Brain.HasPrimaryWeaponEquipped)
            {
                trackGoalLevel += NoWeaponTrackGoalLevel;
            }
            else
            {
                //Faudra rajouter weapon strength
                trackGoalLevel += 0.7f;
            }
            return trackGoalLevel;
        }

        private float EvaluateBagValue(float distanceToItem, Item item)
        {

            float bagValueLevel = 0.0f;

            Bag newBag = (Bag)item;
            float nonEquippedBagValueLevel = newBag.Capacity;
            nonEquippedBagValueLevel = nonEquippedBagValueLevel / actor.Brain.BagCapacityMaximum;

            float equippedBagValueLevel = actor.Brain.BagCapacityRatio;
            if (equippedBagValueLevel < nonEquippedBagValueLevel)
            {
                bagValueLevel = BagGoalFactor * nonEquippedBagValueLevel / distanceToItem;
            }
            else
            {
                bagValueLevel = BagGoalFactor * (1 - actor.Brain.BagCapacityRatio) * (nonEquippedBagValueLevel) / distanceToItem;
            }

            return bagValueLevel;
        }

        private float EvaluateHealValue(float distanceToItem, Item item)
        {

            float healValueLevel = 0.0f;
            Heal heal = (Heal)item;
            float healStrengthValueLevel = heal.Efficacity / actor.Brain.HealEfficiencyMaximum;
            healValueLevel = HealGoalFactor * (1 - actor.Brain.HealthRatio) * (1 - actor.Brain.HealNumberStorageRatio) * healStrengthValueLevel / distanceToItem;
            return healValueLevel;
        }
        private float EvaluateBoostValue(float distanceToItem, Item item)
        {

            float boostValueLevel = 0.0f;
            Boost boost = (Boost)item;
            float boostStrengthValueLevel = boost.Efficacity / actor.Brain.BoostEfficiencyMaximum;
            boostValueLevel = BoostGoalFactor * (1 - actor.Brain.HealthRatio) * (1 - actor.Brain.BoostNumberStorageRatio) * boostStrengthValueLevel / distanceToItem;
            return boostValueLevel;
        }
        private float EvaluateVestValue(float distanceToItem, Item item)
        {

            float vestValueLevel = 0.0f;

            Vest newVest = (Vest)item;
            float nonEquippedVestValueLevel = newVest.ProtectionValue;
            nonEquippedVestValueLevel = nonEquippedVestValueLevel / actor.Brain.VestProtectionMaximum;

            float equippedVestValueLevel = actor.Brain.VestProtectionRatio;


            if (equippedVestValueLevel < nonEquippedVestValueLevel)
            {
                vestValueLevel = VestGoalFactor * nonEquippedVestValueLevel / distanceToItem;
            }
            else
            {
                vestValueLevel = VestGoalFactor * (1 - actor.Brain.VestProtectionRatio) * (nonEquippedVestValueLevel) / distanceToItem;
            }

            return vestValueLevel;
        }
        private float EvaluateHelmetValue(float distanceToItem, Item item)
        {

            float helmetValueLevel = 0.0f;

            Helmet newHelmet = (Helmet)item;
            float nonEquippedHelmetValueLevel = newHelmet.ProtectionValue;
            nonEquippedHelmetValueLevel = nonEquippedHelmetValueLevel / actor.Brain.HelmetProtectionMaximum;

            float equippedHelmetValueLevel = actor.Brain.HelmetProtectionRatio;


            if (equippedHelmetValueLevel < nonEquippedHelmetValueLevel)
            {
                helmetValueLevel = HelmetGoalFactor * nonEquippedHelmetValueLevel / distanceToItem;
            }
            else
            {
                helmetValueLevel = HelmetGoalFactor * (1 - actor.Brain.HelmetProtectionRatio) * (nonEquippedHelmetValueLevel) / distanceToItem;
            }

            return helmetValueLevel;
        }
        private float EvaluateGrenadeValue(float distanceToItem, Item item)
        {

            float grenadeValueLevel = 0.0f;

            //grenade = kt grenade number deja la???*(1-helemetpower)*(1-health)*(1-vestpower)/disttogrenade
            return grenadeValueLevel;
        }

        private float EvaluateAmmoPackValue(float distanceToItem, Item item)
        {

            float ammoPackValueLevel = 0.0f;
            //si on a la weapon augmente value selon force weapon???
            AmmoPack ammoPack = (AmmoPack)item;
            ammoPackValueLevel = AmmoPackGoalFactor * (1 - actor.Brain.GetAmmoPackStorageRatio(ammoPack.AmmoType)) / distanceToItem;
            return ammoPackValueLevel;
        }

        private float EvaluateWeaponValue(float distanceToItem, Item item)
        {
            float weaponValueLevel = 0.0f;

            //si si a weapon ou pas
            //weapon = kw * (health*(1-weaponStrength))/DistToWeapon
            //level ammo
            //type weapon
            return weaponValueLevel;
        }
    }
}

