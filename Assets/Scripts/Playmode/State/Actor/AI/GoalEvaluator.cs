
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



        public GoalEvaluator(ActorAI actor)
        {
            this.actor = actor;
        }

        private void EvaluateInventoryValue()
        {

        }

        private void EvaluateFoundItemsValues()
        {

        }

        public float EvaluateLootGoal()
        {
            //par type d'item possiblement
            float lootGoalLevel = 0.0f;
  
            //Loot factor average les 
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

        private float EvaluateHealValue()
        {
          
            float healValueLevel = 0.0f;
            //heal = kh *(1-health)/disttohealth
             return healValueLevel;
        }

        private float EvaluateVestValue()
        {
           
            float vestValueLevel = 0.0f;
              //vest = kv *(1-vestpower)*(1-health)*(1-helemetpower)/disttovest
            return vestValueLevel;
        }
        private float EvaluateHelmetValue()
        {
            
            float helmetValueLevel = 0.0f;
            //helemt = kt *(1-helemetpower)*(1-health)*(1-vestpower)/disttohelemy
            return helmetValueLevel;
        }
        private float EvaluateWeaponValue()
        {
            float weaponValueLevel = 0.0f;
           
            //weapon = kw * (health*(1-weaponStrength))/DistToWeapon
             //level ammo
            //type weapon
            return weaponValueLevel;
        }
    }
}

