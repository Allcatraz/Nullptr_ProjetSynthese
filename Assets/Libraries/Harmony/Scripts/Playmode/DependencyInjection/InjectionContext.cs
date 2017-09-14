using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Harmony.Injection
{
    /// <summary>
    /// Représente un contexte d'injection, c'est-à-dire la source des dépendances à injecter.
    /// </summary>
    /// <remarks>
    /// Un InjectionContext n'est pas nécessairement la seule source de dépendances. Parfois, l'objet où il y a injection
    /// contient suffisamment d'informations sur sont environnement pour servir lui même de source de
    /// dépendances.
    /// </remarks>
    public interface InjectionContext
    {
        /// <summary>
        /// Liste des fabriques de wrappers de dépendances pour ce InjectionContext. Les fabriques de wrappers peuvent créer 
        /// des Bridges vers d'autre objets ou même des décorateurs.
        /// </summary>
        [NotNull]
        IEnumerable<WrapperFactory> DependencyWrappers { get; }

        /// <summary>
        /// Liste des objets statiques pour ce InjectionContext.
        /// </summary>
        [NotNull]
        IEnumerable<object> StaticComponents { get; }

        /// <summary>
        /// Retourne tous les GameObject ayant le Tag spécifié dans ce InjectionContext.
        /// </summary>
        /// <param name="tag">Tag à rechercher.</param>
        /// <returns>Tous les objets ayant le Tag spécifé.</returns>
        [NotNull]
        IList<GameObject> FindGameObjectsWithTag([NotNull] string tag);
    }
}