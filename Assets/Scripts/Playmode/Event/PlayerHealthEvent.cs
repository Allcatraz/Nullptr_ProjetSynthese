using Harmony;

namespace ProjetSynthese
{
    public class PlayerHealthEvent : IEvent
    {
        public Health PlayerHealth { get; private set; }
        public int OldHealthPoints { get; private set; }
        public int NewHealthPoints { get; private set; }

        public PlayerHealthEvent(Health playerHealth, int oldHealthPoints, int newHealthPoints)
        {
            PlayerHealth = playerHealth;
            OldHealthPoints = oldHealthPoints;
            NewHealthPoints = newHealthPoints;
        }
    }
}