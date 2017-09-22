using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/DamageOnHit")]
    public class DamageOnHit : GameScript
    {
        private Health health;
        private HitSensor hitSensor;

        private void InjectDamageOnHit([EntityScope] Health health,
                                      [EntityScope] HitSensor hitSensor)
        {
            this.health = health;
            this.hitSensor = hitSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectDamageOnHit");
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
            health.Hit(hitPoints);
        }
    }
}