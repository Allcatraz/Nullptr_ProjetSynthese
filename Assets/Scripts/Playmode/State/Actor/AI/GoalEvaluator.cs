
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

        float[] inventoryValues = null;
        float[] foundItemsValues = null;

        public enum ItemOwningStatus { Ground, Inventory }

        public GoalEvaluator(ActorAI actor)
        {
            this.actor = actor;
        }

        private void EvaluateInventoryValue()
        {
            Item item = null;
            inventoryValues = new float[actor.AIInventory.ListInventory.Count];
            int i = 0;
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
                        inventoryValues[i] = EvaluateWeaponValue(ItemOwningStatus.Inventory);
                        break;
                    case ItemType.Grenade:
                        inventoryValues[i] = EvaluateGrenadeValue(ItemOwningStatus.Inventory);
                        break;
                    case ItemType.Helmet:
                        inventoryValues[i] = EvaluateHelmetValue(ItemOwningStatus.Inventory);
                        break;
                    case ItemType.Vest:
                        inventoryValues[i] = EvaluateVestValue(ItemOwningStatus.Inventory);
                        break;
                    case ItemType.Bag:
                        inventoryValues[i] = EvaluateBagValue(ItemOwningStatus.Inventory);
                        break;
                    case ItemType.Heal:
                        inventoryValues[i] = EvaluateHealValue(ItemOwningStatus.Inventory);
                        break;
                    case ItemType.Boost:
                        inventoryValues[i] = EvaluateBoostValue(ItemOwningStatus.Inventory);
                        break;
                    case ItemType.AmmoPack:
                        inventoryValues[i] = EvaluateAmmoPackValue(ItemOwningStatus.Inventory);
                        break;
                    default:
                        break;
                }
                i++;
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

        private float EvaluateBagValue(ItemOwningStatus itemOwningStatus)
        {

            float bagValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //bag = kh *(1-health)/disttobag ???espace restant inventaire
            //si deja un bag ....
            return bagValueLevel;
        }

        private float EvaluateHealValue(ItemOwningStatus itemOwningStatus)
        {

            float healValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //heal = kh *(1-health)/disttohealth
            return healValueLevel;
        }
        private float EvaluateBoostValue(ItemOwningStatus itemOwningStatus)
        {

            float boostValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //??????boost = kh *(1-health)/disttoboost?????
            return boostValueLevel;
        }
        private float EvaluateVestValue(ItemOwningStatus itemOwningStatus)
        {

            float vestValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //vest = kv *(1-vestpower)*(1-health)*(1-helemetpower)/disttovest
            return vestValueLevel;
        }
        private float EvaluateHelmetValue(ItemOwningStatus itemOwningStatus)
        {

            float helmetValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //helemt = kt *(1-helemetpower)*(1-health)*(1-vestpower)/disttohelemy
            return helmetValueLevel;
        }
        private float EvaluateGrenadeValue(ItemOwningStatus itemOwningStatus)
        {

            float grenadeValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //grenade = kt grenade number deja la???*(1-helemetpower)*(1-health)*(1-vestpower)/disttogrenade
            return grenadeValueLevel;
        }

        private float EvaluateAmmoPackValue(ItemOwningStatus itemOwningStatus)
        {

            float ammoPackValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //Ammo = kt ammo number deja la???*(1-weponstrengthassociépower)*(1-health)*(1-vestpower)/disttoammopack
            return ammoPackValueLevel;
        }

        private float EvaluateWeaponValue(ItemOwningStatus itemOwningStatus)
        {
            float weaponValueLevel = 0.0f;
            float distanceToItem = 0.0f;
            if (itemOwningStatus == ItemOwningStatus.Inventory)
            {
                distanceToItem = 1.0f;
            }
            //si si a weapon ou pas
            //weapon = kw * (health*(1-weaponStrength))/DistToWeapon
            //level ammo
            //type weapon
            return weaponValueLevel;
        }
        //fonction calcul health ratio
        //fonction calcul weaponStrength
        //fonctin calcul protection strenght
        //TODO
    }
}

