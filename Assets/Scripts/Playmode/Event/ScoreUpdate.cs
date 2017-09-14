using Harmony.EventSystem;
using Harmony.Testing;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    public class ScoreUpdate : IUpdate
    {
        public Score Score { get; private set; }

        public ScoreUpdate(Score score)
        {
            Score = score;
        }
    }
}