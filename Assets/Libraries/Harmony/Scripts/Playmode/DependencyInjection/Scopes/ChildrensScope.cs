using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau Enfants.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects dans le GameObject ciblé, incluant les enfants de ses enfants, sauf lui-même.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject ciblé, incluant ses enfants et les enfants de ses enfants, sauf lui-même.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ChildrensScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IScript target)
        {
            return target.GetAllChildrens();
        }

        protected override IList<object> GetEligibleDependencies(IScript target, Type dependencyType)
        {
            return new List<object>(target.GetComponentsInChildren(dependencyType)).Filter(delegate(object item)
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