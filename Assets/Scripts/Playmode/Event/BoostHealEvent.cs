using Harmony;

namespace ProjetSynthese
{
    public class BoostHealEvent : IEvent
    {
        public float HealthPoints { get; set; }

        public BoostHealEvent(float healthPoints)
        {
            HealthPoints = healthPoints;
        }
    }
}