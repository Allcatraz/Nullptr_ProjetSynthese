using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class DeathCircleTimeLeftEvent : IEvent
    {
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }
        public bool IsWaitFinish { get; private set; }

        public DeathCircleTimeLeftEvent(int minutes, int seconds, bool isWaitFinish)
        {
            Minutes = minutes;
            Seconds = seconds;
            IsWaitFinish = isWaitFinish;
        }
    }
}