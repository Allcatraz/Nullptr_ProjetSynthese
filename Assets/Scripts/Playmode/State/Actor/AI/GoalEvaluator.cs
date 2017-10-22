
using UnityEngine;

namespace ProjetSynthese
{
    public class GoalEvaluator
    {

        private readonly AIBrain brain;

        private const float NoWeaponTrackGoalLevel = 0.0f;
        private const float EmptyInventoryLootGoalLevel = 1.0f;
        private const float TrackFactor = 1.0f;
        private const float LootFactor = 1.0f;

        public GoalEvaluator(AIBrain brain)
        {
            this.brain = brain;
        }

        public float EvaluateLootGoal()
        {
            //par type d'item possiblement
            float lootGoalLevel = 0.0f;
            //heal = kh *(1-health)/disttohealth
            //weapon = kw * (health*(1-weaponStrength))/DistToWeapon

            //vest = kv *(1-vestpower)*(1-health)*(1-helemetpower)/disttovest
            //helemt = kt *(1-helemetpower)*(1-health)*(1-vestpower)/disttohelemy

            //peu rajouter facteur de protection helemt et vest
            //peu rajouter si inventaire plein, bag, boost
            //effet pas de weapon ou protection base bonus
            //prévoit item précis
            //level ammo
            //type weapon
            if (brain.IsInventoryEmpty())
            {
                lootGoalLevel += EmptyInventoryLootGoalLevel;
            }

            return lootGoalLevel;
        }
        public float EvaluateTrackGoal()
        {
            float trackGoalLevel = 0.0f;
            //voir ci-haut
            //track = ka *health*weaponstrength(vestpower*helmetPower
            //ajoute ammunition level
            if (!brain.HasPrimaryWeaponEquipped)
            {
                trackGoalLevel += NoWeaponTrackGoalLevel;
            }
            
            return trackGoalLevel;
        }
    }
}

