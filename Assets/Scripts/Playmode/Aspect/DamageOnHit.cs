using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/DamageOnHit")]
    public class DamageOnHit : GameScript
    {
        private Health health;
        private HitSensor hitSensor;

        public void InjectDamageOnHit([EntityScope] Health health,
                                      [EntityScope] HitSensor hitSensor)
        {
            this.health = health;
            this.hitSensor = hitSensor;
        }

        public void Awake()
        {
            InjectDependencies("InjectDamageOnHit");
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
            health.Hit(hitPoints);
        }
    }
}