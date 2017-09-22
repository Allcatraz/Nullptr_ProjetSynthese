using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerDeathEventPublisher")]
    public class PlayerDeathEventPublisher : GameScript
    {
        private Health health;
        private PlayerDeathEventChannel eventChannel;

        private void InjectPlayerDeathEventPublisher([EntityScope] Health health,
                                                    [EventChannelScope] PlayerDeathEventChannel eventChannel)
        {
            this.health = health;
            this.eventChannel = eventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerDeathEventPublisher");
        }

        private void OnEnable()
        {
            health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            health.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            eventChannel.Publish(new PlayerDeathEvent());
        }
    }
}