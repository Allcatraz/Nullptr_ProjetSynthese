using UnityEngine;
namespace ProjetSynthese
{
    public class HealthManager
    {
        private readonly ActorAI Actor;

        private const float HealUseThreshold = 0.66f;
        private const float BoostUseThreshold = 0.80f;
        private const float TimeHealUseThreshold = 1.0f;
        private const float TimeBoostUseThreshold = 3.0f;
        private const float TimeErrorTolerance = 0.0001f;
        private float deltaTimeLastBoostUsage = 0.0f;
        private float deltaTimeLastHealUsage = 0.0f;

        public HealthManager(ActorAI actor)
        {
            this.Actor = actor;
        }

        public void TendHealth()
        {
            deltaTimeLastBoostUsage += Time.deltaTime;
            deltaTimeLastHealUsage += Time.deltaTime;
            if (ValidateHealUse())
            {
                Actor.EquipmentManager.UseHeal();
                deltaTimeLastHealUsage = 0.0f;
            }
            if (ValidateBoostUse())
            {
                Actor.EquipmentManager.UseBoost();
                deltaTimeLastBoostUsage = 0.0f;
            }
        }

        private bool ValidateBoostUse()
        {
            bool validBoostUsage = false;
            if (deltaTimeLastHealUsage > TimeErrorTolerance
                && deltaTimeLastBoostUsage > TimeBoostUseThreshold
                && Actor.Brain.HealthRatio < BoostUseThreshold
                && Actor.Brain.InventoryBestBoost != null)
            {
                validBoostUsage = true;
            }

            return validBoostUsage;
        }

        private bool ValidateHealUse()
        {
            bool validHealUsage = false;

            if (Actor.Brain.HealthRatio < HealUseThreshold
                && deltaTimeLastHealUsage > TimeHealUseThreshold
                && Actor.Brain.InventoryBestHeal != null)
            {
                validHealUsage = true;
            }

            return validHealUsage;
        }

    }
}