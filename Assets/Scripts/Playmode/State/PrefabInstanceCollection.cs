using UnityEngine;
using System.Collections.Generic;
using Harmony;

namespace ProjetSynthese
{
    public delegate void PrefabInstanceAddedEventHandler(R.E.Prefab prefab, GameObject instance);
    public delegate void PrefabInstanceRemovedEventHandler(R.E.Prefab prefab, GameObject instance);

    [AddComponentMenu("Game/State/PrefabInstanceCollection")]
    public class PrefabInstanceCollection : GameScript
    {
        [SerializeField]
        private R.E.Prefab prefab;

        public event PrefabInstanceAddedEventHandler OnInstanceAdded;
        public event PrefabInstanceRemovedEventHandler OnInstanceRemoved;

        private CreationEventChannel creationEventChannel;
        private DestroyEventChannel destroyEventChannel;

        private IList<GameObject> instances;

        private void InjectPrefabInstanceCollection([EventChannelScope] CreationEventChannel creationEventChannel,
                                                   [EventChannelScope] DestroyEventChannel destroyEventChannel)
        {
            this.creationEventChannel = creationEventChannel;
            this.destroyEventChannel = destroyEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectPrefabInstanceCollection");

            instances = new List<GameObject>();

            creationEventChannel.OnEventPublished += OnCreation;
            destroyEventChannel.OnEventPublished += OnDestroy;
        }

        private void OnDestroy()
        {
            instances.Clear();

            creationEventChannel.OnEventPublished -= OnCreation;
            destroyEventChannel.OnEventPublished -= OnDestroy;
        }

        public int Count
        {
            get { return instances.Count; }
        }

        public void DestroyAll()
        {
            foreach (GameObject instance in instances)
            {
                Destroy(instance);
            }
            instances.Clear();
        }

        private void OnCreation(CreationEvent creationEvent)
        {
            if (creationEvent.CreatedPrefab == prefab)
            {
                instances.Add(creationEvent.CreatedGameObject);
                if (OnInstanceAdded != null) OnInstanceAdded(creationEvent.CreatedPrefab, creationEvent.CreatedGameObject);
            }
        }

        private void OnDestroy(DestroyEvent destroyEvent)
        {
            if (destroyEvent.DestroyedPrefab == prefab)
            {
                instances.Remove(destroyEvent.DestroyedGameObject);

                if (OnInstanceRemoved != null) OnInstanceRemoved(destroyEvent.DestroyedPrefab, destroyEvent.DestroyedGameObject);
            }
        }
    }
}