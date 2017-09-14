using Harmony;
using Harmony.Injection;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Control/LoadInitialActivity")]
    public class LoadInitialActivity : GameScript
    {
        [SerializeField]
        private UnityActivity activity;

        private IActivityStack activityStack;

        public void InjectLoadInitialActivity(UnityActivity activity,
                                              [ApplicationScope] IActivityStack activityStack)
        {
            this.activity = activity;
            this.activityStack = activityStack;
        }

        public void Awake()
        {
            InjectDependencies("InjectLoadInitialActivity", activity);
        }

        public void Start()
        {
            activityStack.StartActivity(activity);
        }
    }
}