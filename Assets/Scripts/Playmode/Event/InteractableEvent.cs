using Harmony;

namespace ProjetSynthese
{
    public class InteractableEvent : IEvent
    {
        public bool FoundInteractable { get; set; }

        public InteractableEvent(bool foundInteractable)
        {
            FoundInteractable = foundInteractable;
        }
    }
}