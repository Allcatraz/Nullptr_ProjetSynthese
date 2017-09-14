using System;
using Harmony.Testing;

namespace Harmony.Injection
{
    /// <summary>
    /// Exception de base pour toutes les exception pouvant survenir lors de l'injection de dépendances.
    /// </summary>
    [NotTested(Reason.Exception)]
    public abstract class DependencyInjectionException : Exception
    {
        protected DependencyInjectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}