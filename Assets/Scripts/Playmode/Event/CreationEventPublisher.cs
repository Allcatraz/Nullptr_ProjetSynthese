using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/CreationEventPublisher")]
    public class CreationEventPublisher : GameScript
    {
        [SerializeField]
        private R.E.Prefab prefab;

        private GameObject topParent;
        private CreationEventChannel eventChannel;

        public void InjectCreationEventPublisher(R.E.Prefab prefab,
                                                 [TopParentScope] GameObject topParent,
                                                 [EventChannelScope] CreationEventChannel eventChannel)
        {
            this.prefab = prefab;
            this.topParent = topParent;
            this.eventChannel = eventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectCreationEventPublisher", prefab);

            eventChannel.Publish(new CreationEvent(prefab, topParent));
        }
    }
}