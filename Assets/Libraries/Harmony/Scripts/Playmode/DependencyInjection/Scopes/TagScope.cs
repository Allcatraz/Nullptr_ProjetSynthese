using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.Injection
{
    /// <summary>
    /// Portée de niveau Tag.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Le GameObject ayant le tag donné.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject ayant le tag donné, incluant ses enfants et les enfants de ses enfants.
    /// </item>
    /// </list>
    /// Plusieurs GameObjects avec le tag donné sont autorisés.
    /// </para>
    /// </remarks>
    public class TagScope : Scope
    {
        private readonly string tagName;

        public TagScope(string tagName)
        {
            this.tagName = tagName;
        }

        protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
        {
            return new List<GameObject>(injectionContext.FindGameObjectsWithTag(tagName));
        }

        protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            IList<GameObject> dependenciesObjects = injectionContext.FindGameObjectsWithTag(tagName);
            if (dependenciesObjects.Count == 0)
            {
                throw new DependencySourceNotFoundException(target, dependencyType, this);
            }
            IList<object> dependencies = new List<object>();
            foreach (GameObject dependencySource in dependenciesObjects)
            {
                foreach (Component dependency in dependencySource.GetComponentsInChildren(dependencyType))
                {
                    dependencies.Add(dependency);
                }
            }
            return dependencies;
        }
    }
}