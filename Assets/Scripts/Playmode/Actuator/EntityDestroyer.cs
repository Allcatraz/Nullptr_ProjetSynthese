using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    public delegate void EntityDestroyedEventHandler();

    [AddComponentMenu("Game/Actuator/EntityDestroyer")]
    public class EntityDestroyer : GameScript
    {
        private GameObject topParent;

        public virtual event EntityDestroyedEventHandler OnDestroyed;

        public void InjectEntityDestroyer([TopParentScope] GameObject topParent)
        {
            this.topParent = topParent;
        }

        private void Awake()
        {
            InjectDependencies("InjectEntityDestroyer");
        }

        [CalledOutsideOfCode]
        public void Destroy()
        {
            topParent.Destroy();

            if (OnDestroyed != null) OnDestroyed();
        }
    }
}