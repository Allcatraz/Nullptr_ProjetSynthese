using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/LoadInitialActivity")]
    public class LoadInitialActivity : GameScript
    {
        [SerializeField]
        private Activity activity;

        private ActivityStack activityStack;

        private void InjectLoadInitialActivity([ApplicationScope] ActivityStack activityStack)
        {
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectLoadInitialActivity");
        }

        private void Start()
        {
            activityStack.StartActivity(activity);
        }
    }
}