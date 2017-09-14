using System.Data.Common;
using System.Data.SQLite;
using Harmony.Database;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/SqLiteParameterFactory")]
    public class SqLiteParameterFactory : Harmony.UnityScript, DbParameterFactory
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