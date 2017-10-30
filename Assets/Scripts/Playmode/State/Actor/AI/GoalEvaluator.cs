
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
        private const float WeaponGoalFactor = 1.0f;

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
                    //peut-être faut vérfier si cell pas null ici et item pas null
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

        }

        public float EvaluateLootGoal()
        {
            //par type d'item possiblement
            float lootGoalLevel = 0.0f;

            //Loot factor average les evalue des items
            //peu rajouter facteur de protection helemt et vest
            //peu rajouter si inventaire plein, bag, boost
            //effet pas de weapon ou protection base bonus

            if (actor.EquipmentManager.IsInventoryEmpty())
            {
                lootGoalLevel += EmptyInventoryLootGoalLevel;
            }

            return lootGoalLevel;
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
            return trackGoalLevel;
        }

        private float EvaluateBagValue(float distanceToItem, Item item)
        {

            float bagValueLevel = 0.0f;

            //bag = kh *(1-health)/disttobag ???espace restant inventaire
            //si deja un bag ....
            return bagValueLevel;
        }

        private float EvaluateHealValue(float distanceToItem, Item item)
        {

            float healValueLevel = 0.0f;
            healValueLevel = HealGoalFactor * (1 - actor.Brain.HealthRatio) * (1 - actor.Brain.HealNumberStorageRatio) / distanceToItem;
            return healValueLevel;
        }
        private float EvaluateBoostValue(float distanceToItem, Item item)
        {

            float boostValueLevel = 0.0f;

            //??????boost = kh *(1-health)/disttoboost?????
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
            nonEquippedHelmetValueLevel = nonEquippedHelmetValueLevel /actor.Brain.HelmetProtectionMaximum;

            float equippedHelmetValueLevel = actor.Brain.HelmetProtectionRatio;

            
            if (equippedHelmetValueLevel < nonEquippedHelmetValueLevel)
            {
                helmetValueLevel = HelmetGoalFactor * nonEquippedHelmetValueLevel / distanceToItem;
            }
            else
            {
                helmetValueLevel = HelmetGoalFactor * (1 - actor.Brain.HelmetProtectionRatio)*(nonEquippedHelmetValueLevel) / distanceToItem;
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

            //Ammo = kt ammo number deja la???*(1-weponstrengthassociépower)*(1-health)*(1-vestpower)/disttoammopack
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
        //fonction calcul weaponStrength
        //TODO
    }
}

