using UnityEngine;
using System.Collections.Generic;
using Harmony;
using Harmony.Injection;

namespace ProjetSynthese
{
    public delegate void PrefabInstanceAddedEventHandler(R.E.Prefab prefab, GameObject instance);
    public delegate void PrefabInstanceRemovedEventHandler(R.E.Prefab prefab, GameObject instance);

    [AddComponentMenu("Game/World/Control/PrefabInstanceCollection")]
    public class PrefabInstanceCollection : GameScript
    {
        [SerializeField]
        private R.E.Prefab prefab;

        public virtual event PrefabInstanceAddedEventHandler OnInstanceAdded;
        public virtual event PrefabInstanceRemovedEventHandler OnInstanceRemoved;

        private IHierachy hierachy;
        private CreationEventChannel creationEventChannel;
        private DestroyEventChannel destroyEventChannel;

        private IList<GameObject> instances;

        public void InjectPrefabInstanceCollection(R.E.Prefab prefab,
                                                   [ApplicationScope] IHierachy hierachy,
                                                   [EventChannelScope] CreationEventChannel creationEventChannel,
                                                   [EventChannelScope] DestroyEventChannel destroyEventChannel)
        {
            this.prefab = prefab;
            this.hierachy = hierachy;
            this.creationEventChannel = creationEventChannel;
            this.destroyEventChannel = destroyEventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectPrefabInstanceCollection", prefab);

            instances = new List<GameObject>();

            creationEventChannel.OnEventPublished += OnCreation;
            destroyEventChannel.OnEventPublished += OnDestroy;
        }

        public void OnDestroy()
        {
            instances.Clear();

            creationEventChannel.OnEventPublished -= OnCreation;
            destroyEventChannel.OnEventPublished -= OnDestroy;
        }

        public virtual int Count
        {
            get { return instances.Count; }
        }

        public virtual void DestroyAll()
        {
            foreach (GameObject instance in instances)
            {
                hierachy.DestroyGameObject(instance);
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