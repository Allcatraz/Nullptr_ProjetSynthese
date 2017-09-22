using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/DestroyOnHitStimulus")]
    public class DestroyOnHitStimulus : GameScript
    {
        private HitStimulus hitStimulus;
        private EntityDestroyer entityDestroyer;

        private void InjectDestroyOnHitStimulus([EntityScope] HitStimulus hitStimulus,
                                               [EntityScope] EntityDestroyer entityDestroyer)
        {
            this.hitStimulus = hitStimulus;
            this.entityDestroyer = entityDestroyer;
        }

        private void Awake()
        {
            InjectDependencies("InjectDestroyOnHitStimulus");
        }

        private void OnEnable()
        {
            hitStimulus.OnHit += OnHit;
        }

        private void OnDisable()
        {
            hitStimulus.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            entityDestroyer.Destroy();
        }
    }
}