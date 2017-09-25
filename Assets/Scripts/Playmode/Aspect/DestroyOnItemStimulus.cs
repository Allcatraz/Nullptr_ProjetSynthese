using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/DestroyOnItemStimulus")]
    public class DestroyOnItemStimulus : GameScript
    {
        private ItemStimulus itemStimulus;
        private EntityDestroyer entityDestroyer;

        private void InjectDestroyOnItemStimulus([EntityScope] ItemStimulus itemStimulus,
                                                 [EntityScope] EntityDestroyer entityDestroyer)
        {
            this.itemStimulus = itemStimulus;
            this.entityDestroyer = entityDestroyer;
        }

        private void Awake()
        {
            InjectDependencies("InjectDestroyOnItemStimulus");
        }

        private void OnEnable()
        {
            itemStimulus.OnItemCollected += OnItemCollected;
        }

        private void OnDisable()
        {
            itemStimulus.OnItemCollected -= OnItemCollected;
        }

        private void OnItemCollected(Effect effect)
        {
            entityDestroyer.Destroy();
        }
    }
}