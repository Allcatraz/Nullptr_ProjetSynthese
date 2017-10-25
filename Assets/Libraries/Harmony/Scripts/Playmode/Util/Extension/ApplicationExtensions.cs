using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour l'application.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Fourni le chemin vers le dossier des données de l'application. Il est uniquement possible de lire dans ce dossier.
        /// </summary>
        [NotNull]
        public static string ApplicationDataPath
        {
            get { return Application.streamingAssetsPath; }
        }

        /// <summary>
        /// Fourni le chemin vers le dossier des sauvegardes de l'application. Il est possible d'écrire et de lire dans ce dossier.
        /// </summary>
        [NotNull]
        public static string PersistentDataPath
        {
            get { return Application.persistentDataPath; }
        }

        /// <summary>
        /// Ferme immédiatement l'application.
        /// </summary>
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}