using System.Data.Common;
using System.Data.SQLite;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/SqLiteParameterFactory")]
    public class SqLiteParameterFactory : GameScript, IDbParameterFactory
    {
        public DbParameter GetParameter(object value)
        {
            return new SQLiteParameter
            {
                Value = value
            };
        }
    }
}