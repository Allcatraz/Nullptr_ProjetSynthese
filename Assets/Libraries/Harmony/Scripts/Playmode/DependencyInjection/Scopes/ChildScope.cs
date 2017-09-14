﻿using System;
using System.Collections.Generic;
using Harmony.Util;
using UnityEngine;

namespace Harmony.Injection
{
    /// <summary>
    /// Portée de niveau Enfants.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette portée permet d'obtenir :
    /// <list type="bullet">
    /// <item>
    /// Un des GameObjects dans le GameObject ciblé, incluant les enfants de ses enfants, mais pas lui-même.
    /// </item>
    /// <item>
    /// Un des Components dans le GameObject ciblé, incluant ses enfants et les enfants de ses enfants, mais pas lui-même.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ChildScope : Scope
    {
        protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
        {
            return target.GetAllChildrens();
        }

        protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
        {
            return new List<object>(target.GetComponentsInChildren(dependencyType)).Filter(delegate(object item)
            {
                Component component = item as Component;
                if (component != null)
                {
                    return component.gameObject != target.gameObject;
                }
                return false;
            });
        }
    }
}