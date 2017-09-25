using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class CreationEvent : IEvent
    {
        public R.E.Prefab CreatedPrefab { get; private set; }
        public GameObject CreatedGameObject { get; private set; }

        public CreationEvent(R.E.Prefab createdPrefab, GameObject createdGameObject)
        {
            CreatedGameObject = createdGameObject;
            CreatedPrefab = createdPrefab;
        }
    }
}