using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/SqLiteConnectionFactory")]
    public class SqLiteConnectionFactory : GameScript, IDbConnectionFactory
    {
        private const string SqliteConnectionTemplate = "URI=file:{0}";

        [SerializeField]
        private string databaseFilePath = "Database.db";

        private string connexionString;

        public void Awake()
        {
            connexionString = String.Format(SqliteConnectionTemplate, Path.Combine(ApplicationExtensions.ApplicationDataPath, databaseFilePath));
        }

        public DbConnection GetConnection()
        {
            return new SQLiteConnection(connexionString);
        }
    }
}