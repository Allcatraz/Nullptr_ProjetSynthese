using Harmony;

namespace ProjetSynthese
{
    public class DeathCircleDistanceEvent : IEvent
    {
        public float SafeCircleRadius { get; set; }
        public float PlayerRadius { get; set; }
        public float DeathCircleRadius { get; set; }

        public DeathCircleDistanceEvent(float safeCircleRadius, float deathCircleRadius, float playerRadius)
        {
            SafeCircleRadius = safeCircleRadius;
            PlayerRadius = playerRadius;
            DeathCircleRadius = deathCircleRadius;
        }
    }
}

