using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/LoadingScreenController")]
    public class LoadingScreenController : GameScript
    {
        private Canvas loadingScreenCanvas;
        private ActivityStack activityStack;

        private void InjectLoadingScreenController([EntityScope] Canvas loadingScreenCanvas,
                                                  [ApplicationScope] ActivityStack activityStack)
        {
            this.loadingScreenCanvas = loadingScreenCanvas;
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectLoadingScreenController");

            loadingScreenCanvas.enabled = false;
            activityStack.OnActivityLoadingStarted += OnActivityLoadStart;
            activityStack.OnActivityLoadingEnded += OnActivityLoadEnd;
        }

        private void OnDestroy()
        {
            activityStack.OnActivityLoadingStarted -= OnActivityLoadStart;
            activityStack.OnActivityLoadingEnded -= OnActivityLoadEnd;
        }

        private void OnActivityLoadStart()
        {
            loadingScreenCanvas.enabled = true;
        }

        private void OnActivityLoadEnd()
        {
            loadingScreenCanvas.enabled = false;
        }
    }
}