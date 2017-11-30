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

        [Tooltip("Le nom du fichier de la base de donnée")]
        [SerializeField]
        private string databaseFileName = "Database.db";

        private string connexionString;

        private void Awake()
        {
            if (DatabaseDoesntExists())
            {
               CreateDatabase();
            }
            connexionString = GetConnexionString();
        }

        public DbConnection GetConnection()
        {
            return new SQLiteConnection(connexionString);
        }

        public void CreateDatabase()
        {
            File.Copy(GetDatabaseFilePath(), GetCurrentDatabaseFilePath(), true);
        }

        public void ResetDatabase()
        {
            string currentDatabaseFilePath = GetCurrentDatabaseFilePath();
            if (File.Exists(currentDatabaseFilePath))
            {
                File.Delete(currentDatabaseFilePath);
            }
        }

        private bool DatabaseDoesntExists()
        {
            return !File.Exists(GetCurrentDatabaseFilePath());
        }

        private string GetDatabaseFilePath()
        {
            return Path.Combine(ApplicationExtensions.ApplicationDataPath, databaseFileName);
        }

        private string GetCurrentDatabaseFilePath()
        {
            return Path.Combine(ApplicationExtensions.PersistentDataPath, databaseFileName);
        }

        private string GetConnexionString()
        {
            return String.Format(SqliteConnectionTemplate, GetCurrentDatabaseFilePath());
        }
    }
}