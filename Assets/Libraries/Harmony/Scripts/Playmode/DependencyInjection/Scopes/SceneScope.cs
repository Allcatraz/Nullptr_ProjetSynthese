using System;
using System.Collections.Generic;
using Harmony.Util;
using UnityEngine;

namespace Harmony.Injection
{
    /// <summary>
    /// Portée de niveau scène.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects dans le GameObject avec le tag « <c>Scene Dependencies</c> » de la scène du GameObject ciblé, incluant 
    /// lui-même et les enfants de ses enfants.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject avec le tag « <c>Scene Dependencies</c> » de la scène du GameObject ciblé, incluant 
    /// ses enfants et les enfants de ses enfants.</item>
    /// </list>
    /// Un seul GameObject par scène avec le tag « <c>Scene Dependencies</c> » est autorisé.
    /// </para>
    /// </remarks>
    public class SceneScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
        {
            return GetDependencySource(injectionContext, target, typeof(GameObject)).GetAllHierachy();
        }

        protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            return new List<object>(GetDependencySource(injectionContext, target, dependencyType).GetComponentsInChildren(dependencyType));
        }

        private GameObject GetDependencySource(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            IList<GameObject> dependenciesObjects = injectionContext.FindGameObjectsWithTag(R.S.Tag.SceneDependencies);
            dependenciesObjects = dependenciesObjects.Filter(gameObject => gameObject.scene == target.gameObject.scene);
            if (dependenciesObjects.Count == 0)
            {
                throw new DependencySourceNotFoundException(target, dependencyType, this);
            }
            if (dependenciesObjects.Count > 1)
            {
                throw new MoreThanOneDependencySourceFoundException(target, dependencyType, this);
            }
            return dependenciesObjects[0];
        }
    }
}