using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
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