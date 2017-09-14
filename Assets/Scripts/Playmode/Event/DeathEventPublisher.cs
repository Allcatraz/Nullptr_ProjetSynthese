using UnityEngine;
using Harmony;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/DeathEventPublisher")]
    public class DeathEventPublisher : GameScript
    {
        [SerializeField]
        private R.E.Prefab prefab;

        private Health health;
        private DeathEventChannel eventChannel;

        public void InjectDeathEventPublisher(R.E.Prefab prefab,
                                              [EntityScope] Health health,
                                              [EventChannelScope] DeathEventChannel eventChannel)
        {
            this.prefab = prefab;
            this.health = health;
            this.eventChannel = eventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectDeathEventPublisher", prefab);
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
            eventChannel.Publish(new DeathEvent(prefab));
        }
    }
}