using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau Parent.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Le GameObject « <c>Parent</c> » du GameObject ciblé.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject « <c>Parent</c> » du GameObject ciblé.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ParentScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IScript target)
        {
            Transform parent = target.GameObject.transform.parent;
            if (parent == null)
            {
                throw new DependencySourceNotFoundException(target, typeof(GameObject), this);
            }
            return new[] { parent.gameObject };
        }

        protected override IList<object> GetEligibleDependencies(IScript target, Type dependencyType)
        {
            Transform parent = target.GameObject.transform.parent;
            if (parent == null)
            {
                throw new DependencySourceNotFoundException(target, dependencyType, this);
            }
            return new List<object>(parent.gameObject.GetComponents(dependencyType));
        }
    }
}