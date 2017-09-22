using UnityEngine;
using System.Collections;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/DestroyAfterTimeOutOfScreen")]
    public class DestroyAfterTimeOutOfScreen : GameScript
    {
        [SerializeField]
        private int delayBeforeDeathInSeconds = 5;

        private Renderer topParentRenderer;
        private EntityDestroyer entityDestroyer;

        private Coroutine destructionCoroutine;

        private void InjectDestroyAfterTimeOutOfScreen([TopParentScope] Renderer topParentRenderer,
                                                      [EntityScope] EntityDestroyer entityDestroyer)
        {
            this.topParentRenderer = topParentRenderer;
            this.entityDestroyer = entityDestroyer;
        }

        private void Awake()
        {
            InjectDependencies("InjectDestroyAfterTimeOutOfScreen");
        }

        private void OnEnable()
        {
            topParentRenderer.Events().OnIsVisible += OnInScreen;
            topParentRenderer.Events().OnIsInvisible += OnOutOfScreen;

            //Do the first update
            if (!topParentRenderer.isVisible)
            {
                OnOutOfScreen();
            }
        }

        private void OnDisable()
        {
            topParentRenderer.Events().OnIsVisible -= OnInScreen;
            topParentRenderer.Events().OnIsInvisible -= OnOutOfScreen;
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
            destructionCoroutine = StartCoroutine(DestroyAfterDelayRoutine());
        }

        private void CancelDestruction()
        {
            if (destructionCoroutine != null)
            {
                StopCoroutine(destructionCoroutine);
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