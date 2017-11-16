using System.Data.Common;

namespace ProjetSynthese
{
    public class PlayerMapper : SqLiteDataMapper<Player>
    {
        public override Player GetObjectFromReader(DbDataReader reader)
        {
            return new Player
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Password = reader.GetString(reader.GetOrdinal("password"))
            };
        }
    }
}

