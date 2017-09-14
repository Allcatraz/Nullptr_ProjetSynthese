using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Représente l'état global de l'application.
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Fourni le chemin vers le dossier des données de l'application. Il est possible d'écrire et de lire dans
        /// ce dossier.
        /// </summary>
        [NotNull]
        string ApplicationDataPath { get; }

        /// <summary>
        /// Ferme immédiatement l'application.
        /// </summary>
        void Quit();
    }
}