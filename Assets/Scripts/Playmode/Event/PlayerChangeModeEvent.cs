using Harmony;

namespace ProjetSynthese
{
    public class PlayerChangeModeEvent : IEvent
    {
        public bool IsPlayerInFirstPerson { get; set; }

        public PlayerChangeModeEvent(bool isPlayerInFirstPerson)
        {
            IsPlayerInFirstPerson = isPlayerInFirstPerson;
        }
    }
}
