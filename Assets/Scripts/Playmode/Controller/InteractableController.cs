using System.Collections;
using System.Collections.Generic;
using Harmony;
using ProjetSynthese;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class InteractableController : GameScript
    {
        private Text text;
        private InteractableEventChannel interactableEventChannel;

        private void InjectInterractableController([GameObjectScope] Text text,
                                                   [EventChannelScope] InteractableEventChannel interactableEventChannel)
        {
            this.interactableEventChannel = interactableEventChannel;
            this.text = text;
        }

        private void Awake()
        {
            InjectDependencies("InjectInterractableController");
            interactableEventChannel.OnEventPublished += OnInteractableFound;
        }

        private void OnDestroy()
        {
            interactableEventChannel.OnEventPublished -= OnInteractableFound;
        }

        private void OnInteractableFound(InteractableEvent interactableEvent)
        {
            text.text = "Interact (Press " + ActionKey.Instance.Interact + ")";
            text.gameObject.SetActive(interactableEvent.FoundInteractable);
        }
    }
}