using Harmony.EventSystem;
using Harmony.Testing;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    public class PlayerHealthUpdate : IUpdate
    {
        public Health PlayerHealth { get; private set; }

        public PlayerHealthUpdate(Health playerHealth)
        {
            PlayerHealth = playerHealth;
        }
    }
}