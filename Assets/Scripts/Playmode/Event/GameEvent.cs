using Harmony;

namespace ProjetSynthese
{
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