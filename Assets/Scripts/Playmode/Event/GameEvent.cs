using Harmony.EventSystem;
using Harmony.Testing;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    public class GameEvent : IEvent
    {
        public bool HasGameEnded { get; private set; }
        public Score Score { get; private set; }

        public GameEvent(bool hasGameEnded, Score score)
        {
            HasGameEnded = hasGameEnded;
            Score = score;
        }
    }
}