namespace Harmony
{
    /// <summary>
    /// Représente et gère le temps au sein de l'application.
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// Délai écoulé entre deux images dans le jeu. En secondes (donc, ce nombre est à virgule flotante).
        /// </summary>
        float DeltaTime { get; }

        /// <summary>
        /// Délai entre deux mises à jour du moteur physique.
        /// </summary>
        float FixedDeltaTime { get; }

        /// <summary>
        /// Démarre le flux du temps. Le temps s'écoule à nouveau.
        /// </summary>
        void Resume();

        /// <summary>
        /// Met le temps en pause. Le temps ne s'écoule plus.
        /// </summary>
        void Pause();

        /// <summary>
        /// Indique si, oui ou non, le flux du temps est arrêté.
        /// </summary>
        /// <returns>Vrai si le temps s'écoule normalement, faux si le temps ne s'écoule plus.</returns>
        bool IsPaused();
    }
}