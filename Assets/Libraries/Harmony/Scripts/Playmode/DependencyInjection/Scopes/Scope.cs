using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente une portée d'injection. Une portée est un sous-ensemble de tous les objets du jeu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Durant l'injection de dépendance, pour limiter le nombre de candidats pouvant être utilisé pour combler
    /// une dépendance, <see cref="Injector"/> utilise les portées. Tout dépendance doit posséder une portée,
    /// sans quoi, il risque d'y avoir plus d'un candidat pour la même dépendance, et donc, l'injecteur ne pourra
    /// pas choisir entre les deux à cause de l'ambiguité.
    /// </para>
    /// <para>
    /// Advenant le cas où la portée est insufisante pour réduire le nombre de candidats à 1, utilisez les 
    /// <see cref="Filter">Filters</see>.
    /// </para>
    /// </remarks>
    /// <seealso cref="Filter"/>
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class Scope : Attribute
    {
        /// <summary>
        /// Retourne les dépendances du type demandé dans ce Scope.
        /// </summary>
        /// <param name="target">
        /// Objet où les dépendances doivent être injectées. Ce n'est pas le Scope qui fait l'injection : ce paramêtre sert seulement
        /// de suplément d'information.
        /// </param>
        /// <param name="dependencyType">Type de la dépendance à obtenir.</param>
        /// <returns></returns>
        [NotNull]
        public IList<object> GetDependencies([NotNull] IScript target, [NotNull] Type dependencyType)
        {
            //GAME OBJECT DEPENDENCIES
            if (dependencyType == typeof(GameObject))
            {
                return GetEligibleGameObjects(target).Convert<GameObject, object>();
            }

            //NORMAL DEPENDENCIES
            return GetEligibleDependencies(target, dependencyType);
        }

        protected abstract IList<GameObject> GetEligibleGameObjects([NotNull] IScript target);

        protected abstract IList<object> GetEligibleDependencies([NotNull] IScript target,
                                                                 [NotNull] Type dependencyType);
    }
}