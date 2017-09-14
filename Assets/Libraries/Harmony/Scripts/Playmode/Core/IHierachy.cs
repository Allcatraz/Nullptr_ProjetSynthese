using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Représente la structure de l'univers du jeu.
    /// </summary>
    public interface IHierachy
    {
        /// <summary>
        /// Indique si, oui ou non, un scène du nom spécifié est actuellement chargée et active.
        /// </summary>
        /// <param name="sceneName">Nom de la scène.</param>
        /// <returns>Vrai si la scène est chargée et active, faux sinon.</returns>
        bool IsSceneLoaded([NotNull] string sceneName);

        /// <summary>
        /// Charge immédiatement la scène spécifiée. Si la scène est déjà chargée, elle n'est pas rechargée.
        /// </summary>
        /// <param name="sceneName">Nom de la scène à charger.</param>
        /// <param name="mode">Mode de chargement de la scène.</param>
        /// <remarks>
        /// Ne pas appeller durant le jeu. Préférer plutôt utiliser <see cref="IActivityStack"/>.
        /// </remarks>
        void LoadScene([NotNull] string sceneName, LoadSceneMode mode);

        /// <summary>
        /// Décharge immédiatement la scène spécifiée.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <remarks>
        /// Ne pas appeller durant le jeu. Préférer plutôt utiliser <see cref="IActivityStack"/>.
        /// </remarks>
        void UnloadScene([NotNull] string sceneName);

        /// <summary>
        /// Marque une scène comme étant active. La scène active est celle dans laquelle les nouveau
        /// objets seront crées. Elle est aussi généralement considérée comme la scène principale,
        /// les autres scènes servant de support à la scène principale.
        /// </summary>
        /// <param name="scene">Scène à marquer comme étant active.</param>
        /// <returns></returns>
        void SetActiveScene(Scene scene);

        /// <summary>
        /// Détruit le GameObject envoyé en paramêtre, ainsi que tous ses enfants, ses Components
        /// et les Components de ses enfants. Toute Couroutine attachés à l'un de ces éléments
        /// est aussi arrêtée et déruite.
        /// </summary>
        /// <param name="gameObject">GameObject à détruire.</param>
        void DestroyGameObject([NotNull] GameObject gameObject);

        /// <summary>
        /// Trouve tous les GameObjects ayant le tag donné au sein du jeu.
        /// </summary>
        /// <param name="tag">Tag à rechercher.</param>
        /// <returns>Liste de tous les GameObjects ayant le tag donné.</returns>
        [NotNull]
        IList<GameObject> FindGameObjectsWithTag([NotNull] string tag);
    }
}