using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/BoostHealEventChannel")]
    public class BoostHealEventPublisher : GameScript
    {
        private BoostStats boostStats;
        private BoostHealEventChannel boostHealEventChannel;

        private void InjectBoostHealEventPublisher([GameObjectScope] BoostStats boostStats,
                                                   [EventChannelScope] BoostHealEventChannel boostHealEventChannel)
        {
            this.boostStats = boostStats;
            this.boostHealEventChannel = boostHealEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectBoostHealEventPublisher");
        }

        private void OnEnable()
        {
            boostStats.OnBoostHeal += OnBoostHeal;
        }

        private void OnDisable()
        {
            boostStats.OnBoostHeal -= OnBoostHeal;
        }

        private void OnBoostHeal(float healtPoints)
        {
            boostHealEventChannel.Publish(new BoostHealEvent(healtPoints));
        }
    }
}
