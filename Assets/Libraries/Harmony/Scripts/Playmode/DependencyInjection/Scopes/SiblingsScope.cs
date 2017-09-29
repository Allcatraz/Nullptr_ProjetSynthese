using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Portée de niveau Frères et soeurs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des ses frères et soeurs du GameObject ciblé.
    /// </item>
    /// <item>
    /// Un des Components dans un des ses frères et soeurs du GameObject ciblé.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class SiblingsScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IScript target)
        {
            return target.GetAllSiblings();
        }

        protected override IList<object> GetEligibleDependencies(IScript target, Type dependencyType)
        {
            return new List<object>(target.GetComponentsInSiblings(dependencyType));
        }
    }
}