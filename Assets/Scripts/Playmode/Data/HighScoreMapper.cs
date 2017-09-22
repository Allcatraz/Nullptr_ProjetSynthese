using System.Data.Common;

namespace ProjetSynthese
{
    public class HighScoreMapper : SqLiteDataMapper<HighScore>
    {
        public override HighScore GetObjectFromReader(DbDataReader reader)
        {
            return new HighScore
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                ScorePoints = (uint) reader.GetInt32(reader.GetOrdinal("score"))
            };
        }
    }
}