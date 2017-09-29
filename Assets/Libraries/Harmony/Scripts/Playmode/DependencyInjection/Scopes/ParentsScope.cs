using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau Parents.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// L'un des GameObjects « <c>Parent</c> » du GameObject ciblé, sauf lui même.
    /// </item>
    /// <item>
    /// Un des Components dans l'un des GameObjects « <c>Parent</c> » du GameObject ciblé, sauf lui-même.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ParentsScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IScript target)
        {
            return target.GetAllParents();
        }

        protected override IList<object> GetEligibleDependencies(IScript target, Type dependencyType)
        {
            return new List<object>(target.GetComponentsInParent(dependencyType)).Filter(delegate(object item)
            {
                Component component = item as Component;
                if (component != null)
                {
                    return component.gameObject != target.GameObject;
                }
                return false;
            });
        }
    }
}