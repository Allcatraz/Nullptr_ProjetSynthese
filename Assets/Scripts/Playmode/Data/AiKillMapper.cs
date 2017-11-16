using System.Data.Common;

namespace ProjetSynthese
{
    public class AiKillMapper : SqLiteDataMapper<AiKill>
    {

        public override AiKill GetObjectFromReader(DbDataReader reader)
        {
            return new AiKill
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("playerId"))
            };
        }
    }
}