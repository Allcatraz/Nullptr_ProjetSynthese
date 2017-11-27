using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class DeathCircleDistanceEvent : IEvent
    {
        public float SafeCircleRadius { get; set; }
        public Vector3 Center { get; set; }
        public float DeathCircleRadius { get; set; }

        public DeathCircleDistanceEvent(float safeCircleRadius, float deathCircleRadius, Vector3 center)
        {
            SafeCircleRadius = safeCircleRadius;
            Center = center;
            DeathCircleRadius = deathCircleRadius;
        }
    }
}

