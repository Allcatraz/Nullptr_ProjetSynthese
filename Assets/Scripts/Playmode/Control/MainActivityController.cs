using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/MainActivityController")]
    public class MainActivityController : GameScript, IActivityController
    {
        [SerializeField]
        private Menu mainMenu;

        private ActivityStack activityStack;

        private void InjectMainActivityController([ApplicationScope] ActivityStack activityStack)
        {
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectMainActivityController");
        }

        public void OnCreate()
        {
            activityStack.StartMenu(mainMenu);
        }

        public void OnStop()
        {
            //Nothing to do
        }
    }
}