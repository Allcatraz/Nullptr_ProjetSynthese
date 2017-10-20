using Harmony;

namespace ProjetSynthese
{
    public class PlayerBoostEvent : IEvent
    {
        public BoostStats PlayerBoost { get; private set; }
        public float OldBoostPoints { get; private set; }
        public float NewBoostPoints { get; private set; }

        public PlayerBoostEvent(BoostStats playerBoost, float oldBoostPoints, float newBoostPoints)
        {
            PlayerBoost = playerBoost;
            OldBoostPoints = oldBoostPoints;
            NewBoostPoints = newBoostPoints;
        }
    }
}
