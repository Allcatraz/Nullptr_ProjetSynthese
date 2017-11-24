using System.Data.Common;

namespace ProjetSynthese
{
    public class ProtectionOfPlayerMapper : SqLiteDataMapper<ProtectionOfPlayer>
    {
        public override ProtectionOfPlayer GetObjectFromReader(DbDataReader reader)
        {
            return new ProtectionOfPlayer
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("playerId")),
                LevelProtection = reader.GetInt32(reader.GetOrdinal("levelProtection")),
                TypeProtection = reader.GetString(reader.GetOrdinal("typeProtection"))
            };
        }
    }
}