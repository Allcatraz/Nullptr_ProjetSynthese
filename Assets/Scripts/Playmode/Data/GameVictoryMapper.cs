using System.Data.Common;

namespace ProjetSynthese
{
    public class GameVictoryMapper : SqLiteDataMapper<GameVictory>
    {
        public override GameVictory GetObjectFromReader(DbDataReader reader)
        {
            return new GameVictory
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("playerId"))
            };
        }
    }
}