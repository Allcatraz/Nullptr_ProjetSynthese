using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Harmony;
using Harmony.Database;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/SqLiteConnectionFactory")]
    public class SqLiteConnectionFactory : GameScript, DbConnectionFactory
    {
        private const string SqliteConnectionTemplate = "URI=file:{0}";

        [SerializeField]
        private string databaseFilePath = "Database.db";

        private IApplication application;

        private string connexionString;

        public void InjectSqLiteConnectionFactory(string databaseFilePath, [ApplicationScope] IApplication application)
        {
            this.databaseFilePath = databaseFilePath;
            this.application = application;
        }

        public void Awake()
        {
            InjectDependencies("InjectSqLiteConnectionFactory", databaseFilePath);

            connexionString = String.Format(SqliteConnectionTemplate, Path.Combine(application.ApplicationDataPath, databaseFilePath));
        }

        public DbConnection GetConnection()
        {
            return new SQLiteConnection(connexionString);
        }
    }
}