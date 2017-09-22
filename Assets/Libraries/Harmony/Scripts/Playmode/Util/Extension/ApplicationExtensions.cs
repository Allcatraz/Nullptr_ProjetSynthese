using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour l'application.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Fourni le chemin vers le dossier des données de l'application. Il est possible d'écrire et de lire dans
        /// ce dossier.
        /// </summary>
        [NotNull]
        public static string ApplicationDataPath
        {
            get { return UnityEngine.Application.streamingAssetsPath; }
        }

        /// <summary>
        /// Ferme immédiatement l'application.
        /// </summary>
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}