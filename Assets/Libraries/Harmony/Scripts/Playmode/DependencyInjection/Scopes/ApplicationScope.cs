using System;
using System.Collections.Generic;
using Harmony.Util;
using UnityEngine;

namespace Harmony.Injection
{
    /// <summary>
    /// Portée de niveau Application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects dans le GameObject avec le tag « <c>Application Dependencies</c> », incluant lui-même et les enfants
    /// de ses enfants.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject avec le tag « <c>Application Dependencies</c> », incluant ses enfants et les enfants
    /// de ses enfants.
    /// </item>
    /// </list>
    /// Un seul GameObject avec le tag « <c>Application Dependencies</c> » est autorisé dans tout le projet.
    /// </para>
    /// </remarks>
    public class ApplicationScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
        {
            return GetDependencySource(injectionContext, target, typeof(GameObject)).GetAllHierachy();
        }

        protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            //First, try getting it from the static dependencies
            foreach (object staticComponent in injectionContext.StaticComponents)
            {
                if (dependencyType.IsInstanceOfType(staticComponent))
                {
                    return new List<object>(new[] {staticComponent});
                }
            }
            //Else, try getting it on the GameObject that has the ApplicationDependencies Tag
            return new List<object>(GetDependencySource(injectionContext, target, dependencyType).GetComponentsInChildren(dependencyType));
        }

        private GameObject GetDependencySource(IInjectionContext injectionContext, UnityScript owner, Type dependencyType)
        {
            IList<GameObject> dependenciesObjects = injectionContext.FindGameObjectsWithTag(R.S.Tag.ApplicationDependencies);
            if (dependenciesObjects.Count == 0)
            {
                throw new DependencySourceNotFoundException(owner, dependencyType, this);
            }
            if (dependenciesObjects.Count > 1)
            {
                throw new MoreThanOneDependencySourceFoundException(owner, dependencyType, this);
            }
            return dependenciesObjects[0];
        }
    }
}