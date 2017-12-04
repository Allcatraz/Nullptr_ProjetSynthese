using System.Data.Common;

namespace ProjetSynthese
{
    public class PlayedGameMapper : SqLiteDataMapper<PlayedGame>
    {
        public override PlayedGame GetObjectFromReader(DbDataReader reader)
        {
            return new PlayedGame
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("playerId"))
            };
        }
    }
}