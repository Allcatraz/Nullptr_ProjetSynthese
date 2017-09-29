using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau Application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects avec le tag « <c>Application Dependencies</c> ».
    /// </item>
    /// <item>
    /// Un des Components dans un des GameObjects avec le tag « <c>Application Dependencies</c> », incluant ses enfants et les enfants
    /// de ses enfants.
    /// </item>
    /// </list>
    /// Plusieurs GameObjects avec le tag « <c>Application Dependencies</c> » sont autorisés dans le projet.
    /// </para>
    /// </remarks>
    public class ApplicationScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IScript target)
        {
            return GetDependencySources(target, typeof(GameObject));
        }

        protected override IList<object> GetEligibleDependencies(IScript target, Type dependencyType)
        {
            IList<object> dependencies = new List<object>();
            foreach (GameObject dependencySource in GetDependencySources(target, dependencyType))
            {
                foreach (Component dependency in dependencySource.GetComponentsInChildren(dependencyType))
                {
                    dependencies.Add(dependency);
                }
            }
            return dependencies;
        }

        private IList<GameObject> GetDependencySources(IScript target, Type dependencyType)
        {
            IList<GameObject> dependencySources = GameObject.FindGameObjectsWithTag(R.S.Tag.ApplicationDependencies);
            if (dependencySources.Count == 0)
            {
                throw new DependencySourceNotFoundException(target, dependencyType, this);
            }
            return dependencySources;
        }
    }
}