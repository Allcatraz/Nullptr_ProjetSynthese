using Harmony.EventSystem;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerHealthEventPublisher")]
    public class PlayerHealthEventPublisher : GameScript
    {
        private Health health;
        private PlayerHealthEventChannel eventChannel;

        public void InjectPlayerHealthEventPublisher([EntityScope] Health health,
                                                     [EventChannelScope] PlayerHealthEventChannel eventChannel)
        {
            this.health = health;
            this.eventChannel = eventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayerHealthEventPublisher");
        }

        public void OnEnable()
        {
            health.OnHealthChanged += OnHealthChanged;
            eventChannel.OnUpdateRequested += OnRequestUpdate;
        }

        public void OnDisable()
        {
            health.OnHealthChanged -= OnHealthChanged;
            eventChannel.OnUpdateRequested -= OnRequestUpdate;
        }

        private void OnRequestUpdate(EventChannelUpdateHandler<PlayerHealthUpdate> updateHandler)
        {
            updateHandler(new PlayerHealthUpdate(health));
        }

        private void OnHealthChanged(int oldHealthPoints, int newHealthPoints)
        {
            eventChannel.Publish(new PlayerHealthEvent(health, oldHealthPoints, newHealthPoints));
        }
    }
}