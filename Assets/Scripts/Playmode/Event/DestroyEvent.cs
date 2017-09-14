using Harmony;
using Harmony.EventSystem;
using Harmony.Testing;
using UnityEngine;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    public class DestroyEvent : IEvent
    {
        public R.E.Prefab DestroyedPrefab { get; private set; }
        public GameObject DestroyedGameObject { get; private set; }

        public DestroyEvent(R.E.Prefab destroyedPrefab, GameObject destroyedGameObject)
        {
            DestroyedGameObject = destroyedGameObject;
            DestroyedPrefab = destroyedPrefab;
        }
    }
}