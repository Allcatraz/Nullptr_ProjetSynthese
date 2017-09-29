using Harmony;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    public class ChangePlayerScene : GameScript
    {
        private ActivityStack activityStack;

        private void InjectChangePlayerScene([ApplicationScope] ActivityStack activityStack)
        {
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectChangePlayerScene");
        }

        private void Update()
        {
            if (!activityStack.HasActivityLoading())
            {
                SceneManager.MoveGameObjectToScene(gameObject.GetRoot(), SceneManager.GetSceneByName(R.S.Scene.GameFragment));
                Destroy(this);
            }
        }
    }
}

