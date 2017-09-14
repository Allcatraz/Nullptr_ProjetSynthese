﻿using System;
using Harmony.Testing;

namespace Harmony.Injection
{
    /// <summary>
    /// Lancée lorsqu'une dépendance n'as pas été trouvée dans la portée demandée.
    /// </summary>
    [NotTested(Reason.Exception)]
    public class DependencyNotFoundException : DependencyInjectionException
    {
        private const string MessageTemplate = "Dependency of type \"{0}\" needed for component \"{1}\" of GameObject " +
                                               "named \"{2}\" cannot be found in scope \"{3}\".";

        public DependencyNotFoundException(UnityScript target, Type dependencyType, Scope scope)
            : this(target, dependencyType, scope, null)
        {
        }

        public DependencyNotFoundException(UnityScript target, Type dependencyType, Scope scope, Exception innerException)
            : base(String.Format(MessageTemplate,
                                 dependencyType.Name,
                                 target.GetType().Name,
                                 target.name,
                                 scope.GetType().Name),
                   innerException)
        {
        }
    }
}