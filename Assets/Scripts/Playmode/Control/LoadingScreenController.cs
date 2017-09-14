using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Control/LoadingScreenController")]
    public class LoadingScreenController : GameScript
    {
        private ICanvas loadingScreenCanvas;
        private IActivityStack activityStack;

        public void InjectLoadingScreenController([EntityScope] ICanvas loadingScreenCanvas,
                                                  [ApplicationScope] IActivityStack activityStack)
        {
            this.loadingScreenCanvas = loadingScreenCanvas;
            this.activityStack = activityStack;
        }

        public void Awake()
        {
            InjectDependencies("InjectLoadingScreenController");

            loadingScreenCanvas.Enabled = false;
        }

        public void OnEnable()
        {
            activityStack.OnActivityLoadingStarted += OnActivityLoadStart;
            activityStack.OnActivityLoadingEnded += OnActivityLoadEnd;
        }

        public void OnDisable()
        {
            activityStack.OnActivityLoadingStarted -= OnActivityLoadStart;
            activityStack.OnActivityLoadingEnded -= OnActivityLoadEnd;
        }

        private void OnActivityLoadStart()
        {
            loadingScreenCanvas.Enabled = true;
        }

        private void OnActivityLoadEnd()
        {
            loadingScreenCanvas.Enabled = false;
        }
    }
}