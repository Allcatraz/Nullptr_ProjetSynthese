using UnityEngine;
using System.Collections;
using Harmony;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/DestroyAfterDelay")]
    public class DestroyAfterDelay : GameScript
    {
        [SerializeField]
        private int delayBeforeDeathInSeconds;

        private EntityDestroyer entityDestroyer;
        private ICoroutineExecutor coroutineExecutor;

        public void InjectDestroyAfterDelay(int delayBeforeDeathInSeconds,
                                           [EntityScope] EntityDestroyer entityDestroyer,
                                           [ApplicationScope] ICoroutineExecutor coroutineExecutor)
        {
            this.delayBeforeDeathInSeconds = delayBeforeDeathInSeconds;
            this.entityDestroyer = entityDestroyer;
            this.coroutineExecutor = coroutineExecutor;
        }

        public void Awake()
        {
            InjectDependencies("InjectDestroyAfterDelay", delayBeforeDeathInSeconds);
        }

        public void Start()
        {
            coroutineExecutor.StartCoroutine(this, DestroyAfterDelayRoutine());
        }

        private IEnumerator DestroyAfterDelayRoutine()
        {
            yield return new WaitForSeconds(delayBeforeDeathInSeconds);
            entityDestroyer.Destroy();
        }
    }
}