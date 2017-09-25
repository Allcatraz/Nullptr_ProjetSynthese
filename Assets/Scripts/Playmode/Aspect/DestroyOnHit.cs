using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/DestroyOnHit")]
    public class DestroyOnHit : GameScript
    {
        private HitSensor hitSensor;
        private EntityDestroyer entityDestroyer;

        private void InjectDestroyOnHit([EntityScope] HitSensor hitSensor,
                                       [EntityScope] EntityDestroyer entityDestroyer)
        {
            this.hitSensor = hitSensor;
            this.entityDestroyer = entityDestroyer;
        }

        private void Awake()
        {
            InjectDependencies("InjectDestroyOnHit");
        }

        private void OnEnable()
        {
            hitSensor.OnHit += OnHit;
        }

        private void OnDisable()
        {
            hitSensor.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            entityDestroyer.Destroy();
        }
    }
}