using System.Data.Common;

namespace ProjetSynthese
{
    public class PlayerKillMapper : SqLiteDataMapper<PlayerKill>
    {
        public override PlayerKill GetObjectFromReader(DbDataReader reader)
        {
            return new PlayerKill
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("playerId"))
            };
        }
    }
}