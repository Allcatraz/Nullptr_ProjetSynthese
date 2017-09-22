using Harmony;

namespace ProjetSynthese
{
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