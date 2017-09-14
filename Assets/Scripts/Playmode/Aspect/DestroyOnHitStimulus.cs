using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/DestroyOnHitStimulus")]
    public class DestroyOnHitStimulus : GameScript
    {
        private HitStimulus hitStimulus;
        private EntityDestroyer entityDestroyer;

        public void InjectDestroyOnHitStimulus([EntityScope] HitStimulus hitStimulus,
                                               [EntityScope] EntityDestroyer entityDestroyer)
        {
            this.hitStimulus = hitStimulus;
            this.entityDestroyer = entityDestroyer;
        }

        public void Awake()
        {
            InjectDependencies("InjectDestroyOnHitStimulus");
        }

        public void OnEnable()
        {
            hitStimulus.OnHit += OnHit;
        }

        public void OnDisable()
        {
            hitStimulus.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            entityDestroyer.Destroy();
        }
    }
}