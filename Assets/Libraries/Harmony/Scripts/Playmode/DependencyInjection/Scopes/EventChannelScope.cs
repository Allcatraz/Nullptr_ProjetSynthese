using System;
using System.Collections.Generic;
using Harmony.Util;
using UnityEngine;

namespace Harmony.Injection
{
    /// <summary>
    /// Portée de niveau Canal d'événements.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects dans les GameObjects avec le tag « <c>Event Channels</c> », incluant eux-mêmes et les enfants
    /// de ses enfants.
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
        protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
        {
            IList<GameObject> gameObjects = new List<GameObject>();
            foreach (GameObject gameObject in GetDependencySources(injectionContext, target, typeof(GameObject)))
            {
                foreach (GameObject childrenGameObject in gameObject.GetAllHierachy())
                {
                    gameObjects.Add(childrenGameObject);
                }
            }
            return gameObjects;
        }

        protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            IList<object> dependencies = new List<object>();
            foreach (GameObject dependencySource in GetDependencySources(injectionContext, target, dependencyType))
            {
                foreach (Component dependency in dependencySource.GetComponentsInChildren(dependencyType))
                {
                    dependencies.Add(dependency);
                }
            }
            return dependencies;
        }

        private IList<GameObject> GetDependencySources(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            IList<GameObject> dependenciesObjects = injectionContext.FindGameObjectsWithTag(R.S.Tag.EventChannels);
            if (dependenciesObjects.Count == 0)
            {
                throw new DependencySourceNotFoundException(target, dependencyType, this);
            }
            return dependenciesObjects;
        }
    }
}