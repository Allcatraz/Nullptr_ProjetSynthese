using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

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
#if UNITY_EDITOR
            Activity currentPreloadedActivity = GetCurrentPreloadedActivityInEditor();
            if (currentPreloadedActivity != null)
            {
                activityStack.StartPreloadedActivity(currentPreloadedActivity);
            }
            else
            {
                if (SceneManager.sceneCount > 1)
                {
                    Debug.LogWarning("Are you trying to start the game without an activity ? " +
                                     "You have two options : load only the \"Main\" scene in the editor or load all the scenes " +
                                     "of an activity in the editor.");
                }
                activityStack.StartActivity(activity);
            }
#else
            activityStack.StartActivity(activity);
#endif
        }

#if UNITY_EDITOR

        private Activity GetCurrentPreloadedActivityInEditor()
        {
            foreach (Activity activity in AssetsExtensions.FindAssets<Activity>())
            {
                bool hasAllScenesLoaded = true;

                if (activity.Scene != R.E.Scene.None)
                {
                    hasAllScenesLoaded &= SceneManagerExtensions.IsSceneLoaded(R.S.Scene.ToString(activity.Scene));
                }

                foreach (Fragment fragment in activity.Fragments)
                {
                    if (fragment.Scene != R.E.Scene.None)
                    {
                        hasAllScenesLoaded &= SceneManagerExtensions.IsSceneLoaded(R.S.Scene.ToString(fragment.Scene));
                    }
                }

                foreach (Menu menu in activity.Menus)
                {
                    if (menu.Scene != R.E.Scene.None)
                    {
                        hasAllScenesLoaded &= SceneManagerExtensions.IsSceneLoaded(R.S.Scene.ToString(menu.Scene));
                    }
                }

                if (hasAllScenesLoaded)
                {
                    return activity;
                }
            }
            return null;
        }

#endif
    }
}