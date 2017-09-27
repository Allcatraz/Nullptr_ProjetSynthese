using System;
using System.Collections.Generic;
using Harmony.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Représente une pile d'<see cref="Activity"/>. Permet de démarer/arrêter/redémarer des activités.
    /// </summary>
    [AddComponentMenu("Game/Activity/ActivityStack")]
    public class ActivityStack : Script
    {
        private readonly Stack<StackedActivity> activityStack;
        private readonly Stack<StackedMenu> menuStack;
        private readonly IList<string> scenesToLoadRemaining;
        private readonly IList<string> scenesToUnloadRemaining;

        /// <summary>
        /// Événement déclanché lorsque le chargement d'une activité débute.
        /// </summary>
        public event ActivityLoadingEventHandler OnActivityLoadingStarted;

        /// <summary>
        /// Événement déclanché lorsque le chargement des activités est terminé. Autrement dit, il n'y
        /// aucune activité en cours de chargement.
        /// </summary>
        public event ActivityLoadingEventHandler OnActivityLoadingEnded;

        private bool isLoadingActivity;

        /// <summary>
        /// Crée un nouveau ActivityStack vide.
        /// </summary>
        public ActivityStack()
        {
            activityStack = new Stack<StackedActivity>();
            menuStack = new Stack<StackedMenu>();

            scenesToLoadRemaining = new List<string>();
            scenesToUnloadRemaining = new List<string>();
            isLoadingActivity = false;
        }

        /// <summary>
        /// Charge une activité et l'ajoute sur le dessus de la pile d'activités.
        /// </summary>
        /// <param name="activity">Activité à charger.</param>
        /// <remarks>
        /// <para>
        /// Seule l'activité sur le dessus de la pile est conservée. Autrement dit, s'il y a une activité en cours lors
        /// de l'appel à cette méthode, cette dernière est déchargée avant que la nouvelle soit chargée.
        /// </para>
        /// <para>
        /// Le chargement des Activités est asynchrone. Utilisez les évènements <see cref="OnActivityLoadingStarted"/>
        /// et <see cref="OnActivityLoadingEnded"/> pour être notifié du début et de la fin du chargement d'une activité.
        /// </para>
        /// <para>
        /// Au démarrage, IActivityStack cherche pour un <see cref="IActivityController"/> dans le GameObject supposé contenir
        /// le contrôleur de l'activité. S'il en trouve un, il appelle la méthode <see cref="IActivityController.OnCreate"/> dessus.
        /// </para>
        /// </remarks>
        public void StartActivity(Activity activity)
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

        /// <summary>
        /// Redémarre l'activité courante, c'est-à-dire celle sur le dessus de la pile d'activités. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Rien ne se produit s'il n'y a aucune activité en cours.
        /// </para>
        /// <para>
        /// Le rechargement des Activités est asynchrone. Utilisez les évènements <see cref="OnActivityLoadingStarted"/> et 
        /// <see cref="OnActivityLoadingEnded"/> pour être notifié du début et de la fin du chargement asynchrone d'une activité.
        /// </para>
        /// <para>
        /// Au redémarrage, IActivityStack cherche pour un <see cref="IActivityController"/> dans le GameObject supposé contenir
        /// le contrôleur de l'activité. S'il en trouve un, il appelle la méthode <see cref="IActivityController.OnCreate"/> dessus.
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// Décharge l'activité sur le dessus de la pile et la retire de la pile d'activités.
        /// La nouvelle activité sur le dessus de la pile se retrouve alors rechargée. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Si aucune activité est chargée, l'application est tout simplement arrêtée.
        /// </para>
        /// <para>
        /// Le déchargement des Activités est asynchrone. Utilisez les évènements <see cref="OnActivityLoadingStarted"/>
        /// et <see cref="OnActivityLoadingEnded"/> pour être notifié du début et de la fin du chargement d'une activité.
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// Indique si, oui ou non, il y a une activité en cours.
        /// </summary>
        /// <returns>True s'il y a une activité en cours, false sinon.</returns>
        public bool HasActivityRunning()
        {
            return activityStack.Count > 0;
        }

        /// <summary>
        /// Active le menu donné et l'ajoute sur le dessus de la pile de menus.
        /// </summary>
        /// <param name="menu">Menu à activer.</param>
        /// <param name="parameters">Paramêtres à envoyer au menu, si nécessaire.</param>
        /// <remarks>
        /// <para>
        /// Seul le menu sur le dessus de la pile est actif. Tous les menus sur la pile sont affichés en même temps.
        /// </para>
        /// <para>
        /// Pour qu'un menu puisse être démarré, ce dernier doit être chargé. Assurez-vous donc les menus 
        /// utilisés par l'activités soient chargés avant de les utiliser.
        /// </para>
        /// <para>
        /// Au démarrage d'un menu, IMenuStack cherche pour un <see cref="IMenuController"/> dans la scène
        /// du menu. S'il en trouve un, il appelle les méthodes <see cref="IMenuController.OnCreate"/> et 
        /// <see cref="IMenuController.OnResume"/> dessus.
        /// </para>
        /// <para>
        /// S'il y avait déjà un menu d'actif, IMenuStack appellera la méthode <see cref="IMenuController.OnPause"/> sur
        /// le contrôleur de l'ancien menu avant d'activer le nouveau menu.
        /// </para>
        /// </remarks>
        public void StartMenu(Menu menu, params object[] parameters)
        {
            if (HasMenuRunning())
            {
                PauseCurrentMenu();
            }

            PushMenu(menu, parameters);

            ShowCurrentMenu();
        }

        /// <summary>
        /// Désactive le menu sur le dessus de la pile et la retire de la pile de menus.
        /// Le nouveau menu sur le dessus de la pile se retrouve alors activé, s'il y en a un. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Rien ne se produit s'il n'y a aucun menu actif.
        /// </para>
        /// <para>
        /// Lors de l'arrêt d'un menu, IMenuStack cherche pour un <see cref="IMenuController"/> dans la scène
        /// du menu. S'il en trouve un, il appelle les méthodes <see cref="IMenuController.OnPause"/> et 
        /// <see cref="IMenuController.OnStop"/> dessus.
        /// </para>
        /// <para>
        /// Si suivant le retrait du menu courant de la pile il existe toujours un menu dans cette pile, IMenuStack 
        /// appellera la méthode <see cref="IMenuController.OnResume"/> sur le contrôleur de ce menu.
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// Indique si, oui ou non, il y a un menu en cours.
        /// </summary>
        /// <returns>True s'il y a un menu en cours, false sinon.</returns>
        public bool HasMenuRunning()
        {
            return menuStack.Count > 0;
        }

        private void PushActivity(Activity activity)
        {
            activityStack.Push(new StackedActivity(activity));

            ClearMenus();
        }

        private void PopActivity()
        {
            activityStack.Pop();

            ClearMenus();
        }

        private void PushMenu(Menu menu, object[] parameters)
        {
            if (menu.IsAlwaysVisible())
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
                    throw new ArgumentException("Unable to load Activity : scene named \"" + sceneName +
                                                "\" is allready loaded and " +
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
                    Debug.LogWarning("Problem while stopping current Activity : scene named \"" + sceneName +
                                     "\" is not loaded, " +
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
            private readonly Activity activity;
            private readonly IList<StackedFragment> fragments;
            private readonly IList<StackedMenu> menus;

            private IActivityController controller;

            public StackedActivity(Activity activity)
            {
                this.activity = activity;
                fragments = new List<StackedFragment>();
                menus = new List<StackedMenu>();

                foreach (Fragment fragment in activity.Fragments)
                {
                    fragments.Add(new StackedFragment(fragment));
                }

                int currentMenuIndex = 0;
                foreach (Menu menu in activity.Menus)
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

            public StackedMenu GetMenu(Menu menu)
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

                if (activity.ActiveFragmentOnLoad != null)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(R.S.Scene.ToString(activity.ActiveFragmentOnLoad.Scene)));
                }

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
                        throw new ArgumentException(
                            "Unable to find controller for Activity : no IActivityController exists "
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
            private readonly Fragment fragment;

            private IFragmentController controller;

            public StackedFragment(Fragment fragment)
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
                        throw new ArgumentException(
                            "Unable to find controller for Fragment : no IFragmentController exists "
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
            private readonly Menu menu;
            private readonly int menuIndex;
            private readonly int nbMenusTotal;

            private object[] parameters;
            private Canvas canvas;
            private GameObject topParent;
            private IMenuController controller;

            public StackedMenu(Menu menu, int menuIndex, int nbMenusTotal)
            {
                this.menu = menu;
                this.menuIndex = menuIndex;
                this.nbMenusTotal = nbMenusTotal;
            }

            public bool Is(Menu menu)
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

                if (menu.IsAlwaysVisible())
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
                canvas.sortingOrder = (menu.IsAlwaysVisible() ? 0 : nbMenusTotal) + orderInStack + 1;

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

namespace Harmony.Unity
{
    /// <summary>
    /// Signature de toute fonction désirant être notifié d'un chargement d'activité.
    /// </summary>
    public delegate void ActivityLoadingEventHandler();
}