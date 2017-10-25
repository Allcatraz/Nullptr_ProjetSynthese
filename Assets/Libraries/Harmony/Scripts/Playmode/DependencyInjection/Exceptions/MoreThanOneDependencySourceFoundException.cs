using System;

namespace Harmony
{
    /// <summary>
    /// Lancée lorsque plus d'un GameObject sert à contenir les dépendances pour une portée donnée.
    /// </summary>
    public class MoreThanOneDependencySourceFoundException : DependencyInjectionException
    {
        private const string MessageTemplate = "More than one GameObject exists to provide dependencies for \"{0}\" scope. Dependency of type \"{1}\" " +
                                               "needed for component \"{2}\" in GameObject named \"{3}\" cannot be resolved in scope \"{0}\".";

        public MoreThanOneDependencySourceFoundException(IScript target, Type dependencyType, Scope scope)
            : this(target, dependencyType, scope, null)
        {
        }

        public MoreThanOneDependencySourceFoundException(IScript target, Type dependencyType, Scope scope, Exception innerException)
            : base(String.Format(MessageTemplate,
                                 scope.GetType().Name,
                                 dependencyType.Name,
                                 target.GetType().Name,
                                 target.Name
                   ),
                   innerException)
        {
        }
    }
}