using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau scène.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects avec le tag « <c>Scene Dependencies</c> » de la scène du GameObject ciblé.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject avec le tag « <c>Scene Dependencies</c> » de la scène du GameObject ciblé, incluant 
    /// ses enfants et les enfants de ses enfants.</item>
    /// </list>
    /// Plusieurs GameObjects avec le tag « <c>Scene Dependencies</c> » sont autorisés dans une scène. Cependant, il est recommandé d'en
    /// avoir qu'un seul.
    /// </para>
    /// </remarks>
    public class SceneScope : Scope
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
            IList<GameObject> dependencySources = GameObject.FindGameObjectsWithTag(R.S.Tag.SceneDependencies);
            dependencySources = dependencySources.Filter(gameObject => gameObject.scene == target.GameObject.scene);
            if (dependencySources.Count == 0)
            {
                throw new DependencySourceNotFoundException(target, dependencyType, this);
            }
            return dependencySources;
        }
    }
}