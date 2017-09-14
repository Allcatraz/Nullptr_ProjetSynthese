using UnityEngine;
using Harmony;
using Harmony.Injection;
using Harmony.Util;

namespace ProjetSynthese
{
    public delegate void EntityDestroyedEventHandler();

    [AddComponentMenu("Game/World/Object/Actuator/EntityDestroyer")]
    public class EntityDestroyer : GameScript
    {
        private GameObject topParent;
        private IHierachy hierachy;

        public virtual event EntityDestroyedEventHandler OnDestroyed;

        public void InjectEntityDestroyer([TopParentScope] GameObject topParent,
                                          [ApplicationScope] IHierachy hierachy)
        {
            this.topParent = topParent;
            this.hierachy = hierachy;
        }

        public void Awake()
        {
            InjectDependencies("InjectEntityDestroyer");
        }

        [CalledOutsideOfCode]
        public virtual void Destroy()
        {
            hierachy.DestroyGameObject(topParent);

            if (OnDestroyed != null) OnDestroyed();
        }
    }
}