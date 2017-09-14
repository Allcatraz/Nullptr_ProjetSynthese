using UnityEngine;
using System.Collections;
using Harmony;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/DestroyAfterTimeOutOfScreen")]
    public class DestroyAfterTimeOutOfScreen : GameScript
    {
        [SerializeField]
        private int delayBeforeDeathInSeconds = 5;

        private new IRenderer renderer;
        private EntityDestroyer entityDestroyer;
        private ICoroutineExecutor coroutineExecutor;

        private ICoroutine destructionCoroutine;

        public void InjectDestroyAfterTimeOutOfScreen(int delayBeforeDeathInSeconds,
                                                      [TopParentScope] IRenderer renderer,
                                                      [EntityScope] EntityDestroyer entityDestroyer,
                                                      [ApplicationScope] ICoroutineExecutor coroutineExecutor)
        {
            this.delayBeforeDeathInSeconds = delayBeforeDeathInSeconds;
            this.renderer = renderer;
            this.entityDestroyer = entityDestroyer;
            this.coroutineExecutor = coroutineExecutor;
        }

        public void Awake()
        {
            InjectDependencies("InjectDestroyAfterTimeOutOfScreen",
                               delayBeforeDeathInSeconds);
        }

        public void OnEnable()
        {
            renderer.OnBecameVisible += OnInScreen;
            renderer.OnBecameInvisible += OnOutOfScreen;

            //Do the first update
            if (!renderer.IsVisible())
            {
                OnOutOfScreen();
            }
        }

        public void OnDisable()
        {
            renderer.OnBecameVisible -= OnInScreen;
            renderer.OnBecameInvisible -= OnOutOfScreen;
        }

        private void OnOutOfScreen()
        {
            ScheduleDestruction();
        }

        private void OnInScreen()
        {
            CancelDestruction();
        }

        private void ScheduleDestruction()
        {
            CancelDestruction();
            destructionCoroutine = coroutineExecutor.StartCoroutine(this, DestroyAfterDelayRoutine());
        }

        private void CancelDestruction()
        {
            if (destructionCoroutine != null)
            {
                destructionCoroutine.Stop();
                destructionCoroutine = null;
            }
        }

        private IEnumerator DestroyAfterDelayRoutine()
        {
            yield return new WaitForSeconds(delayBeforeDeathInSeconds);
            entityDestroyer.Destroy();
        }
    }
}