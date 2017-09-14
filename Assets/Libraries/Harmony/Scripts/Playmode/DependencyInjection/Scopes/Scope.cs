using System;
using System.Collections.Generic;
using Harmony.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Harmony.Injection
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
        /// <param name="injectionContext">Contexte d'injection, d'où les dépendances sont récupérées.</param>
        /// <param name="target">
        /// Objet où les dépendances doivent être injectées. Ce n'est pas le Scope qui fait l'injection : ce paramêtre sert seulement
        /// de suplément d'information.
        /// </param>
        /// <param name="dependencyType">Type de la dépendance à obtenir.</param>
        /// <returns></returns>
        [NotNull]
        public IList<object> GetDependencies([NotNull] IInjectionContext injectionContext, [NotNull] UnityScript target, [NotNull] Type dependencyType)
        {
            //GAME OBJECT DEPENDENCIES
            if (dependencyType == typeof(GameObject))
            {
                return GetEligibleGameObjects(injectionContext, target).Convert<GameObject, object>();
            }

            //WRAPPED DEPENDENCIES
            WrapperFactory wrapperFactory = GetDependencyWrapperFor(injectionContext, target, dependencyType);
            if (wrapperFactory != null)
            {
                return wrapperFactory.Wrap(GetEligibleDependencies(injectionContext, target, wrapperFactory.GetWrappedType()));
            }

            //NORMAL DEPENDENCIES
            return GetEligibleDependencies(injectionContext, target, dependencyType);
        }

        private WrapperFactory GetDependencyWrapperFor(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            foreach (WrapperFactory dependencyWrapper in injectionContext.DependencyWrappers)
            {
                //Prevent from asking a dependency that has a wrapper type. Must return the wrapper type, not the wrapped type.
                //EX : Can't have a RigidBody2D (The wrapped type). Must return a UnityRigidBody2D (The wrapper type). 
                if (dependencyWrapper.CanWrap(dependencyType))
                {
                    throw new WrapperNotUsedException(target, dependencyType, this, dependencyWrapper.GetWrapperType());
                }
                //Prevent from asking directly for a wrapper type implmementation. Must ask for his abstract type (interface).
                //EX : Can't ask for a UnityRigidBody2D (The wrapper implementation). Must ask for a IRigidBody2D (The wrapper abstraction).
                if (dependencyWrapper.IsWrapperTypeImplementation(dependencyType))
                {
                    throw new WrongWrapperUsedException(target, dependencyType, this, dependencyWrapper.GetWrapperType());
                }
                if (dependencyWrapper.CanWrapInto(dependencyType))
                {
                    return dependencyWrapper;
                }
            }
            return null;
        }

        protected abstract IList<GameObject> GetEligibleGameObjects([NotNull] IInjectionContext injectionContext,
                                                                    [NotNull] UnityScript target);

        protected abstract IList<object> GetEligibleDependencies([NotNull] IInjectionContext injectionContext,
                                                                 [NotNull] UnityScript target,
                                                                 [NotNull] Type dependencyType);
    }
}