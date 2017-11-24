using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class InteractableEventPublisher : GameScript
    {
        private InteractableSensor interactableSensor;
        private InteractableEventChannel interactableEventChannel;

        private void InjectInteractableEventPublisher([GameObjectScope] InteractableSensor interactableSensor,
                                                      [EventChannelScope] InteractableEventChannel interactableEventChannel)
        {
            this.interactableSensor = interactableSensor;
            this.interactableEventChannel = interactableEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectInteractableEventPublisher");
            interactableSensor.OnInteratableFound += OnInteractableFound;
        }

        private void OnDestroy()
        {
            interactableSensor.OnInteratableFound -= OnInteractableFound;
        }

        private void OnInteractableFound(bool foundInteractible)
        {
            interactableEventChannel.Publish(new InteractableEvent(foundInteractible));
        }
    }
}