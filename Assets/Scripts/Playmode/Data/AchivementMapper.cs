using System.Data.Common;

namespace ProjetSynthese
{
    public class AchivementMapper : SqLiteDataMapper<Achivement>
    {
        public override Achivement GetObjectFromReader(DbDataReader reader)
        {
            return new Achivement
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("playerId"))
            };
        }
    }
}