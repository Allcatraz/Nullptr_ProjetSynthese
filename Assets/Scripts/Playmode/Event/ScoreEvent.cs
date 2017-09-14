using Harmony.EventSystem;
using Harmony.Testing;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    public class ScoreEvent : IEvent
    {
        public Score Score { get; private set; }
        public uint OldScorePoints { get; private set; }
        public uint NewScorePoints { get; private set; }

        public ScoreEvent(Score score, uint oldScorePoints, uint newScorePoints)
        {
            Score = score;
            OldScorePoints = oldScorePoints;
            NewScorePoints = newScorePoints;
        }
    }
}