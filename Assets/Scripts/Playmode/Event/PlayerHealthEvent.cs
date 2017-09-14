using Harmony.EventSystem;
using Harmony.Testing;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    public class PlayerHealthEvent : PlayerHealthUpdate, IEvent
    {
        public int OldHealthPoints { get; private set; }
        public int NewHealthPoints { get; private set; }

        public PlayerHealthEvent(Health playerHealth, int oldHealthPoints, int newHealthPoints) : base(playerHealth)
        {
            OldHealthPoints = oldHealthPoints;
            NewHealthPoints = newHealthPoints;
        }
    }
}