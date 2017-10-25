using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau Canal d'événements.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects avec le tag « <c>Event Channels</c> ».
    /// </item>
    /// <item>
    /// Un des Components dans les GameObjects avec le tag « <c>Event Channels</c> », incluant leurs enfants et les enfants
    /// de leurs enfants.
    /// </item>
    /// </list>
    /// Plusieurs GameObjects avec le tag « <c>Event Channels</c> » sont autorisés.
    /// </para>
    /// </remarks>
    public class EventChannelScope : Scope
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
            IList<GameObject> dependencySources = GameObject.FindGameObjectsWithTag(R.S.Tag.EventChannels);
            if (dependencySources.Count == 0)
            {
                throw new DependencySourceNotFoundException(target, dependencyType, this);
            }
            return dependencySources;
        }
    }
}