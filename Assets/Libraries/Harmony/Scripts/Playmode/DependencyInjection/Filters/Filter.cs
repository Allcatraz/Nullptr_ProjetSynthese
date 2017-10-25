using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Attribut pour filtrer les dépendances. À utiliser lorsque plus d'une dépendance du même type existe
    /// dans la même portée.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class Filter : Attribute
    {
        /// <summary>
        /// Filtre la liste de dépendances envoyée.
        /// </summary>
        /// <param name="dependencies">Liste des dépendances à filtrer.</param>
        /// <returns>Dépendances filtrées.</returns>
        [NotNull]
        public IList<object> FilterDependencies([NotNull] IList<object> dependencies)
        {
            return dependencies.Filter(FilterDependency);
        }

        /// <summary>
        /// Indique si, oui ou non, une dépendance doit être conservée.
        /// </summary>
        /// <param name="dependency">Dépendance à filtrer.</param>
        /// <returns>Vrai si la dépendance respecte le critère de filtrage, faux sinon.</returns>
        protected abstract bool FilterDependency([NotNull] object dependency);
    }
}