using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/DestroyEventPublisher")]
    public class DestroyEventPublisher : GameScript
    {
        [SerializeField]
        private R.E.Prefab prefab;

        private GameObject topParent;
        private EntityDestroyer entityDestroyer;
        private DestroyEventChannel eventChannel;

        public void InjectDestroyEventPublisher(R.E.Prefab prefab,
                                                [TopParentScope] GameObject topParent,
                                                [EntityScope] EntityDestroyer entityDestroyer,
                                                [EventChannelScope] DestroyEventChannel eventChannel)
        {
            this.prefab = prefab;
            this.topParent = topParent;
            this.entityDestroyer = entityDestroyer;
            this.eventChannel = eventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectDestroyEventPublisher", prefab);

            entityDestroyer.OnDestroyed += OnEntityDestroyed;
        }

        public void OnDestroy()
        {
            entityDestroyer.OnDestroyed -= OnEntityDestroyed;
        }

        private void OnEntityDestroyed()
        {
            eventChannel.Publish(new DestroyEvent(prefab, topParent));
        }
    }
}