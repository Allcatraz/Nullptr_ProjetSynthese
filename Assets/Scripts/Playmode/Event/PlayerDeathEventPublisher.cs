using UnityEngine;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerDeathEventPublisher")]
    public class PlayerDeathEventPublisher : GameScript
    {
        private Health health;
        private PlayerDeathEventChannel eventChannel;

        public void InjectPlayerDeathEventPublisher([EntityScope] Health health,
                                                    [EventChannelScope] PlayerDeathEventChannel eventChannel)
        {
            this.health = health;
            this.eventChannel = eventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayerDeathEventPublisher");
        }

        public void OnEnable()
        {
            health.OnDeath += OnDeath;
        }

        public void OnDisable()
        {
            health.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            eventChannel.Publish(new PlayerDeathEvent());
        }
    }
}