using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class ChooseSpawnController : GameScript
    {
        private ActivityStack activityStack;

        private void InjectChooseSpawnController([ApplicationScope] ActivityStack activityStack)
        {
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectChooseSpawnController");
        }

        private void Update()
        {

        }
    }
}
