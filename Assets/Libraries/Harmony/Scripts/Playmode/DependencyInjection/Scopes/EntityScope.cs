using System;
using System.Collections.Generic;
using Harmony.Util;
using UnityEngine;

namespace Harmony.Injection
{
    /// <summary>
    /// Portée de niveau Entitée.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects dans le GameObject ciblé, incluant lui-même, ses parents, ses frères et soeurs, ses enfants et les enfants 
    /// de ses enfants.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject ciblé, incluant lui-même, ses parents, ses frères et soeurs, ses enfants et les enfants 
    /// de ses enfants.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class EntityScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
        {
            return target.GetTopParent().GetAllHierachy();
        }

        protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            return new List<object>(target.GetComponentsInChildrensParentsOrSiblings(dependencyType));
        }
    }
}