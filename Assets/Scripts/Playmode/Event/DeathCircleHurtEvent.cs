using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class DeathCircleHurtEvent : IEvent
    {
        public float HurtPoints { get; set; }

        public DeathCircleHurtEvent(float hurtPoints)
        {
            HurtPoints = hurtPoints;
        }
    }
}

