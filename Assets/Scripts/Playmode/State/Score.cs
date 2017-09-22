using UnityEngine;

namespace ProjetSynthese
{
    public delegate void ScoreChangedEventHandler(uint oldScorePoints, uint newScorePoints);

    [AddComponentMenu("Game/State/Score")]
    public class Score : GameScript
    {
        private uint scorePoints;

        public event ScoreChangedEventHandler OnScoreChanged;

        public uint ScorePoints
        {
            get { return scorePoints; }
            private set
            {
                uint oldScorePoints = scorePoints;
                scorePoints = value;
                if (OnScoreChanged != null) OnScoreChanged(oldScorePoints, scorePoints);
            }
        }

        public void AddPoints(uint points)
        {
            ScorePoints += points;
        }

        public void Reset()
        {
            ScorePoints = 0;
        }
    }
}