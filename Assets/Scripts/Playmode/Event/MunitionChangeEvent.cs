using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class MunitionChangeEvent : IEvent
    {
        
        public int Munitions { get; set; }
        public int MunitionsMax { get; set; }

        public MunitionChangeEvent(int munitions, int munitionMax)
        {
            Munitions = munitions;
            MunitionsMax = munitionMax;
        }
    }
}
