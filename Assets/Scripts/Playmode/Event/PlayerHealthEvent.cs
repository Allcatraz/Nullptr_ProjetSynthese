using Harmony;

namespace ProjetSynthese
{
    public class PlayerHealthEvent : IEvent
    {
        public Health PlayerHealth { get; private set; }
        public float OldHealthPoints { get; private set; }
        public float NewHealthPoints { get; private set; }

        public PlayerHealthEvent(Health playerHealth, float oldHealthPoints, float newHealthPoints)
        {
            PlayerHealth = playerHealth;
            OldHealthPoints = oldHealthPoints;
            NewHealthPoints = newHealthPoints;
        }
    }
}