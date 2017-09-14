using Harmony.EventSystem;

namespace Harmony
{
    /// <summary>
    /// Représente un fragment faisant partie d'une activité. Un fragment est une partie indépendante du jeu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Plusieurs fragments peuvent coexister en même temps. Les fragments ne communiquent jamais directement entre eux.
    /// Par contre, il peuvent se servir des <see cref="IEventChannel{T}"/> pour publier des évènements ou être notifiés
    /// d'évènements.
    /// </para>
    /// <para>
    /// Les fragments sont chargés en même temps que l'activité. Ils peuvent posséder un contrôleur de fragment, leur permettant
    /// ainsi d'être notifié d'évènements importants reliés au cycle de vie de l'activité.
    /// </para>
    /// </remarks>
    /// <seealso cref="IActivity"/>
    public interface IFragment 
    {
        /// <summary>
        /// Scène du Realm. (Obligatoire)
        /// </summary>
        R.E.Scene Scene { get; }

        /// <summary>
        /// Identifiant du GameObject contenant le controleur du Realm. (Facultatif)
        /// </summary>
        R.E.GameObject Controller { get; }
    }
}