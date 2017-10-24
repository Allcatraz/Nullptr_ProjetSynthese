using Harmony;

namespace ProjetSynthese
{
    public class PlayerUseEvent : IEvent
    {
        public bool IsDoingOtherThing { get; set; }

        public PlayerUseEvent(bool isDoingOtherThing)
        {
            IsDoingOtherThing = isDoingOtherThing;
        }
    }
}
