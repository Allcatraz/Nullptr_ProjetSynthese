using System;

namespace Harmony
{
    /// <summary>
    /// Lancée lorsque la source de dépendances d'une portée donnée n'as pas été trouvée.
    /// </summary>
    public class DependencySourceNotFoundException : DependencyInjectionException
    {
        private const string MessageTemplate = "No GameObject contains dependencies for \"{0}\" scope. Dependency of type \"{1}\" " +
                                               "needed for component \"{2}\" in GameObject named \"{3}\" cannot be resolved in scope \"{0}\".";

        public DependencySourceNotFoundException(IScript target, Type dependencyType, Scope scope)
            : this(target, dependencyType, scope, null)
        {
        }

        public DependencySourceNotFoundException(IScript target, Type dependencyType, Scope scope, Exception innerException)
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