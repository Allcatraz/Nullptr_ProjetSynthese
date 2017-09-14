using Harmony;
using Harmony.Injection;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Control/MainActivityController")]
    public class MainActivityController : GameScript, IActivityController
    {
        [SerializeField]
        private UnityMenu mainMenu;

        private IMenuStack menuStack;

        public void InjectMainActivityController(UnityMenu mainMenu,
                                                 [ApplicationScope] IMenuStack menuStack)
        {
            this.mainMenu = mainMenu;
            this.menuStack = menuStack;
        }

        public void Awake()
        {
            InjectDependencies("InjectMainActivityController",
                               mainMenu);
        }

        public void OnCreate()
        {
            menuStack.StartMenu(mainMenu);
        }

        public void OnStop()
        {
            //Nothing to do
        }
    }
}