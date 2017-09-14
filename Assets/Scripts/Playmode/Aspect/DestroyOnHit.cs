using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/DestroyOnHit")]
    public class DestroyOnHit : GameScript
    {
        private HitSensor hitSensor;
        private EntityDestroyer entityDestroyer;

        public void InjectDestroyOnHit([EntityScope] HitSensor hitSensor,
                                       [EntityScope] EntityDestroyer entityDestroyer)
        {
            this.hitSensor = hitSensor;
            this.entityDestroyer = entityDestroyer;
        }

        public void Awake()
        {
            InjectDependencies("InjectDestroyOnHit");
        }

        public void OnEnable()
        {
            hitSensor.OnHit += OnHit;
        }

        public void OnDisable()
        {
            hitSensor.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            entityDestroyer.Destroy();
        }
    }
}