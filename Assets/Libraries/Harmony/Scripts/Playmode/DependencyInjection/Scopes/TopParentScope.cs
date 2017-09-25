using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau Supérieur ultime.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Le GameObject « <c>Top Parent</c> » du GameObject ciblé.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject « <c>Top Parent</c> » du GameObject ciblé.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class TopParentScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IScript target)
        {
            return new[] { target.GetTopParent() };
        }

        protected override IList<object> GetEligibleDependencies(IScript target, Type dependencyType)
        {
            return new List<object>(target.GetComponentsInTopParent(dependencyType));
        }
    }
}