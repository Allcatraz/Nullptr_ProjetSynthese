using System;

namespace Harmony
{
    /// <summary>
    /// Exception de base pour toutes les exception pouvant survenir lors de l'injection de dépendances.
    /// </summary>
    public abstract class DependencyInjectionException : Exception
    {
        protected DependencyInjectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}