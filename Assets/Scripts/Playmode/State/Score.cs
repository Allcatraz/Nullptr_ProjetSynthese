using UnityEngine;

namespace ProjetSynthese
{
    public delegate void ScoreChangedEventHandler(uint oldScorePoints, uint newScorePoints);

    [AddComponentMenu("Game/World/State/Score")]
    public class Score : GameScript
    {
        private uint scorePoints;

        public virtual event ScoreChangedEventHandler OnScoreChanged;

        public virtual uint ScorePoints
        {
            get { return scorePoints; }
            private set
            {
                uint oldScorePoints = scorePoints;
                scorePoints = value;
                if (OnScoreChanged != null) OnScoreChanged(oldScorePoints, scorePoints);
            }
        }

        public virtual void AddPoints(uint points)
        {
            ScorePoints += points;
        }

        public virtual void Reset()
        {
            ScorePoints = 0;
        }
    }
}