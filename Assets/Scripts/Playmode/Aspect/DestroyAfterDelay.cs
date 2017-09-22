using UnityEngine;
using System.Collections;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/DestroyAfterDelay")]
    public class DestroyAfterDelay : GameScript
    {
        [SerializeField]
        private int delayBeforeDeathInSeconds;

        private EntityDestroyer entityDestroyer;

        private void InjectDestroyAfterDelay([EntityScope] EntityDestroyer entityDestroyer)
        {
            this.entityDestroyer = entityDestroyer;
        }

        private void Awake()
        {
            InjectDependencies("InjectDestroyAfterDelay");
        }

        private void Start()
        {
            StartCoroutine(DestroyAfterDelayRoutine());
        }

        private IEnumerator DestroyAfterDelayRoutine()
        {
            yield return new WaitForSeconds(delayBeforeDeathInSeconds);
            entityDestroyer.Destroy();
        }
    }
}