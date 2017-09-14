﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony.Injection
{
    /// <summary>
    /// Portée de niveau GameObject.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Le GameObject ciblé.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject ciblé.</item>
    /// </list>
    /// Les enfants et les parents ne sont pas considérés dans cette portée contrairement à d'autres portées telles que 
    /// <see cref="EntityScope"/>.
    /// </para>
    /// </remarks>
    public class GameObjectScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
        {
            return new[] {target.gameObject};
        }

        protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            return new List<object>(target.GetComponents(dependencyType));
        }
    }
}