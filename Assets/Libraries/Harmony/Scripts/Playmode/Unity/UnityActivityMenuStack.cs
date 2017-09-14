using System;
using System.Collections.Generic;
using Harmony.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente une pile d'activités et de menus Unity.
    /// </summary>
    /// <inheritdoc cref="IActivityStack"/>
    /// <inheritdoc cref="IMenuStack"/>
    public class UnityActivityMenuStack : IActivityStack, IMenuStack
    {
        private readonly Stack<StackedActivity> activityStack;
        private readonly Stack<StackedMenu> menuStack;
        private readonly IList<string> scenesToLoadRemaining;
        private readonly IList<string> scenesToUnloadRemaining;

        public event ActivityLoadingEventHandler OnActivityLoadingStarted;
        public event ActivityLoadingEventHandler OnActivityLoadingEnded;

        private bool isLoadingActivity;

        public UnityActivityMenuStack()
        {
            activityStack = new Stack<StackedActivity>();
            menuStack = new Stack<StackedMenu>();

            scenesToLoadRemaining = new List<string>();
            scenesToUnloadRemaining = new List<string>();
            isLoadingActivity = false;
        }

        public void StartActivity(IActivity activity)
        {
            if (HasActivityRunning())
            {
                HideCurrentActivity();
                ScheduleCurrentActivityScenesToUnload();
            }

            PushActivity(activity);

            ScheduleCurrentActivityScenesToLoad();

            StartOrContinueScheduledSceneTasks();
        }

        public void RestartCurrentActivity()
        {
            if (HasActivityRunning())
            {
                HideCurrentActivity();

                ScheduleCurrentActivityScenesToUnload();
                ScheduleCurrentActivityScenesToLoad();

                StartOrContinueScheduledSceneTasks();
            }
        }

        public void StopCurrentActivity()
        {
            if (HasActivityRunning())
            {
                HideCurrentActivity();
                ScheduleCurrentActivityScenesToUnload();

                PopActivity();

                if (HasActivityRunning())
                {
                    ScheduleCurrentActivityScenesToLoad();

                    StartOrContinueScheduledSceneTasks();
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    UnityEngine.Application.Quit();
#endif
                }
            }
        }

        public bool HasActivityRunning()
        {
            return activityStack.Count > 0;
        }

        public void StartMenu(IMenu menu, params object[] parameters)
        {
            if (HasMenuRunning())
            {
                PauseCurrentMenu();
            }

            PushMenu(menu, parameters);

            ShowCurrentMenu();
        }

        public void StopCurrentMenu()
        {
            if (HasMenuRunning())
            {
                HideCurrentMenu();

                PopMenu();

                if (HasMenuRunning())
                {
                    ResumeCurrentMenu();
                }
            }
        }

        public bool HasMenuRunning()
        {
            return menuStack.Count > 0;
        }

        private void PushActivity(IActivity activity)
        {
            activityStack.Push(new StackedActivity(activity));

            ClearMenus();
        }

        private void PopActivity()
        {
            activityStack.Pop();

            ClearMenus();
        }

        private void PushMenu(IMenu menu, object[] parameters)
        {
            if (menu.IsAllwaysVisible())
            {
                throw new ArgumentException("Unable to start Menu : menu is allways visible.");
            }
            if (!HasActivityRunning())
            {
                throw new ArgumentException("Unable to start Menu : no activity running.");
            }
            StackedActivity currentActivity = GetCurrentActivity();
            StackedMenu stackedMenu = currentActivity.GetMenu(menu);
            if (stackedMenu == null)
            {
                throw new ArgumentException("Unable to start Menu : menu is not part of the current activity.");
            }
            stackedMenu.SetParameters(parameters);
            menuStack.Push(stackedMenu);
        }

        private void PopMenu()
        {
            menuStack.Pop();
        }

        private void ClearMenus()
        {
            while (HasMenuRunning())
            {
                StopCurrentMenu();
            }
        }

        private void ScheduleCurrentActivityScenesToLoad()
        {
            foreach (string sceneName in GetCurrentActivity().GetScenes())
            {
                if (IsSceneLoaded(sceneName) && !scenesToUnloadRemaining.Contains(sceneName))
                {
                    scenesToLoadRemaining.Clear(); //Prevent anything from happening
                    scenesToUnloadRemaining.Clear(); //Prevent anything from happening
                    throw new ArgumentException("Unable to load Activity : scene named \"" + sceneName + "\" is allready loaded and " +
                                                "is not scheduled to be unloaded. Can't proceed further.");
                }
                scenesToLoadRemaining.Enqueue(sceneName);
            }
        }

        private void ScheduleCurrentActivityScenesToUnload()
        {
            foreach (string sceneName in GetCurrentActivity().GetScenes())
            {
                if (!IsSceneLoaded(sceneName))
                {
                    Debug.LogWarning("Problem while stopping current Activity : scene named \"" + sceneName + "\" is not loaded, " +
                                     "but belongs to the Activity being closed. You may have unloaded it manually somewhere.");
                }
                else
                {
                    scenesToUnloadRemaining.Enqueue(sceneName);
                }
            }
        }

        private void StartOrContinueScheduledSceneTasks()
        {
            if (!isLoadingActivity && (scenesToUnloadRemaining.Count > 0 || scenesToLoadRemaining.Count > 0))
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                isLoadingActivity = true;

                NotifyActivityLoadStart();
            }
            if (isLoadingActivity)
            {
                if (scenesToUnloadRemaining.Count > 0)
                {
                    SceneManager.UnloadSceneAsync(scenesToUnloadRemaining.Peek());
                }
                else if (scenesToLoadRemaining.Count > 0)
                {
                    SceneManager.LoadSceneAsync(scenesToLoadRemaining.Peek(), LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.sceneLoaded -= OnSceneLoaded;
                    SceneManager.sceneUnloaded -= OnSceneUnloaded;
                    isLoadingActivity = false;

                    ShowCurrentActivity();

                    NotifyActivityLoadEnd();
                }
            }
        }

        private StackedActivity GetCurrentActivity()
        {
            return activityStack.Peek();
        }

        private StackedMenu GetCurrentMenu()
        {
            return menuStack.Peek();
        }

        private void ShowCurrentActivity()
        {
            StackedActivity currentActivity = GetCurrentActivity();
            currentActivity.Initialize();
            currentActivity.OnCreate();
        }

        private void HideCurrentActivity()
        {
            StackedActivity currentActivity = GetCurrentActivity();
            currentActivity.OnStop();
        }

        private void ShowCurrentMenu()
        {
            StackedMenu currentMenu = GetCurrentMenu();
            currentMenu.OnCreate(menuStack.Count);
            currentMenu.OnResume();
        }

        private void HideCurrentMenu()
        {
            StackedMenu currentMenu = GetCurrentMenu();
            currentMenu.OnPause();
            currentMenu.OnStop();
        }

        private void ResumeCurrentMenu()
        {
            GetCurrentMenu().OnResume();
        }

        private void PauseCurrentMenu()
        {
            GetCurrentMenu().OnPause();
        }

        private bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scenesToLoadRemaining.Contains(scene.name))
            {
                //We dont know which scene will be loaded first, so instead of just calling "Dequeue", we call "Remove".
                scenesToLoadRemaining.Remove(scene.name);

                StartOrContinueScheduledSceneTasks();
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (scenesToUnloadRemaining.Contains(scene.name))
            {
                //We dont know which scene will be unloaded first, so instead of just calling "Dequeue", we call "Remove".
                scenesToUnloadRemaining.Remove(scene.name);

                StartOrContinueScheduledSceneTasks();
            }
        }

        private void NotifyActivityLoadStart()
        {
            if (OnActivityLoadingStarted != null) OnActivityLoadingStarted();
        }

        private void NotifyActivityLoadEnd()
        {
            if (OnActivityLoadingEnded != null) OnActivityLoadingEnded();
        }

        private sealed class StackedActivity
        {
            private readonly IActivity activity;
            private readonly IList<StackedFragment> fragments;
            private readonly IList<StackedMenu> menus;

            private IActivityController controller;

            public StackedActivity(IActivity activity)
            {
                this.activity = activity;
                fragments = new List<StackedFragment>();
                menus = new List<StackedMenu>();

                foreach (IFragment fragment in activity.Fragments)
                {
                    fragments.Add(new StackedFragment(fragment));
                }

                int currentMenuIndex = 0;
                foreach (IMenu menu in activity.Menus)
                {
                    menus.Add(new StackedMenu(menu, currentMenuIndex, activity.Menus.Count));
                    currentMenuIndex++;
                }
            }

            public IList<string> GetScenes()
            {
                IList<string> scenes = new List<string>();

                if (activity.Scene != R.E.Scene.None)
                {
                    scenes.Add(R.S.Scene.ToString(activity.Scene));
                }

                foreach (StackedFragment fragment in fragments)
                {
                    scenes.Add(fragment.GetScene());
                }
                foreach (StackedMenu menu in menus)
                {
                    scenes.Add(menu.GetScene());
                }
                return scenes;
            }

            public StackedMenu GetMenu(IMenu menu)
            {
                foreach (StackedMenu stackedMenu in menus)
                {
                    if (stackedMenu.Is(menu))
                    {
                        return stackedMenu;
                    }
                }
                return null;
            }

            public void Initialize()
            {
                controller = null;

                string sceneNameToActivate = R.S.Scene.ToString(activity.ActiveFragmentOnLoad == null
                                                                    ? activity.Scene
                                                                    : activity.ActiveFragmentOnLoad.Scene);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneNameToActivate));

                if (activity.Controller != R.E.GameObject.None)
                {
                    string gameObjectName = R.S.GameObject.ToString(activity.Controller);
                    GameObject gameObject = GameObject.Find(gameObjectName);

                    if (gameObject == null)
                    {
                        throw new ArgumentException("Unable to find controller for Activity : no GameObject of name \""
                                                    + gameObjectName + "\" found.");
                    }

                    controller = gameObject.GetComponentInChildren<IActivityController>();

                    if (controller == null)
                    {
                        throw new ArgumentException("Unable to find controller for Activity : no IActivityController exists "
                                                    + "on GameObject of name \"" + gameObjectName + "\".");
                    }
                }

                foreach (StackedFragment fragment in fragments)
                {
                    fragment.Initialize();
                }

                foreach (StackedMenu menu in menus)
                {
                    menu.Initialize();
                }
            }

            public void OnCreate()
            {
                if (controller != null)
                {
                    controller.OnCreate();
                }

                foreach (StackedFragment fragment in fragments)
                {
                    fragment.OnCreate();
                }
            }

            public void OnStop()
            {
                if (controller != null)
                {
                    controller.OnStop();
                }

                foreach (StackedFragment fragment in fragments)
                {
                    fragment.OnStop();
                }
            }
        }

        private sealed class StackedFragment
        {
            private readonly IFragment fragment;

            private IFragmentController controller;

            public StackedFragment(IFragment fragment)
            {
                this.fragment = fragment;
            }

            public string GetScene()
            {
                return R.S.Scene.ToString(fragment.Scene);
            }

            public void Initialize()
            {
                controller = null;

                if (fragment.Controller != R.E.GameObject.None)
                {
                    string gameObjectName = R.S.GameObject.ToString(fragment.Controller);
                    GameObject gameObject = GameObject.Find(gameObjectName);

                    if (gameObject == null)
                    {
                        throw new ArgumentException("Unable to find controller for Fragment : no GameObject of name \""
                                                    + gameObjectName + "\" found.");
                    }

                    controller = gameObject.GetComponentInChildren<IFragmentController>();

                    if (controller == null)
                    {
                        throw new ArgumentException("Unable to find controller for Fragment : no IFragmentController exists "
                                                    + "on GameObject of name \"" + gameObjectName + "\".");
                    }
                }
            }

            public void OnCreate()
            {
                if (controller != null)
                {
                    controller.OnCreate();
                }
            }

            public void OnStop()
            {
                if (controller != null)
                {
                    controller.OnStop();
                }
            }
        }

        private sealed class StackedMenu
        {
            private readonly IMenu menu;
            private readonly int menuIndex;
            private readonly int nbMenusTotal;

            private object[] parameters;
            private Canvas canvas;
            private GameObject topParent;
            private IMenuController controller;

            public StackedMenu(IMenu menu, int menuIndex, int nbMenusTotal)
            {
                this.menu = menu;
                this.menuIndex = menuIndex;
                this.nbMenusTotal = nbMenusTotal;
            }

            public bool Is(IMenu menu)
            {
                return this.menu == menu;
            }

            public string GetScene()
            {
                return R.S.Scene.ToString(menu.Scene);
            }

            public void SetParameters(object[] parameters)
            {
                this.parameters = parameters;
            }

            public void Initialize()
            {
                topParent = null;
                canvas = null;
                controller = null;

                try
                {
                    Scene scene = SceneManager.GetSceneByName(R.S.Scene.ToString(menu.Scene));
                    foreach (GameObject gameObject in scene.GetRootGameObjects())
                    {
                        canvas = gameObject.GetComponentInChildren<Canvas>();
                        if (canvas != null)
                        {
                            topParent = gameObject;
                            break;
                        }
                    }
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException("Missing menu in current activity.");
                }

                if (canvas == null || topParent == null)
                {
                    throw new ArgumentException("Unable to find Canvas and GameObject for Menu.");
                }

                if (menu.Controller != R.E.GameObject.None)
                {
                    string gameObjectName = R.S.GameObject.ToString(menu.Controller);
                    GameObject gameObject = GameObject.Find(gameObjectName);

                    if (gameObject == null)
                    {
                        throw new ArgumentException("Unable to find controller for Menu : no GameObject of name \""
                                                    + gameObjectName + "\" found.");
                    }

                    controller = gameObject.GetComponentInChildren<IMenuController>();

                    if (controller == null)
                    {
                        throw new ArgumentException("Unable to find controller for Menu : no IMenuController exists " +
                                                    "on GameObject of name \"" + gameObjectName + "\".");
                    }
                }

                if (menu.IsAllwaysVisible())
                {
                    OnCreate(menuIndex);
                    OnResume();
                }
                else
                {
                    topParent.SetActive(false);
                    canvas.sortingOrder = 0;
                }
            }

            public void OnCreate(int orderInStack)
            {
                topParent.SetActive(true);
                canvas.sortingOrder = (menu.IsAllwaysVisible() ? 0 : nbMenusTotal) + orderInStack + 1;

                if (controller != null)
                {
                    controller.OnCreate(parameters);
                }
            }

            public void OnResume()
            {
                topParent.SetActive(true);
                if (controller != null)
                {
                    controller.OnResume();
                }
            }

            public void OnPause()
            {
                topParent.SetActive(false);
                if (controller != null)
                {
                    controller.OnPause();
                }
            }

            public void OnStop()
            {
                topParent.SetActive(false);
                canvas.sortingOrder = 0;

                if (controller != null)
                {
                    controller.OnStop();
                }
            }
        }
    }
}