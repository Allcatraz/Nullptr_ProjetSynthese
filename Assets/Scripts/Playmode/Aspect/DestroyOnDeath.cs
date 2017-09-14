using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/DestroyOnDeath")]
    public class DestroyOnDeath : GameScript
    {
        private Health health;
        private EntityDestroyer entityDestroyer;

        public void InjectDestroyOnDeath([EntityScope] Health health,
                                         [EntityScope] EntityDestroyer entityDestroyer)
        {
            this.health = health;
            this.entityDestroyer = entityDestroyer;
        }

        public void Awake()
        {
            InjectDependencies("InjectDestroyOnDeath");
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
            entityDestroyer.Destroy();
        }
    }
}