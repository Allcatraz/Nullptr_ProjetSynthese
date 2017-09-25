using System;

namespace Harmony
{
    /// <summary>
    /// Lancée lorsque l'injecteur a trouvé plusieurs candidats pour la même dépendance et n'est donc pas en mesure 
    /// d'effectuer l'injection.
    /// </summary>
    public class MoreThanOneDependencyFoundException : DependencyInjectionException
    {
        private const string MessageTemplate = "Dependency of type \"{0}\" needed for component \"{1}\" of GameObject " +
                                               "named \"{2}\" have more than one possible solution in scope \"{3}\". Please " +
                                               "apply a Filter or use any other scope.";


        public MoreThanOneDependencyFoundException(IScript target, Type dependencyType, Scope scope)
            : this(target, dependencyType, scope, null)
        {
        }

        public MoreThanOneDependencyFoundException(IScript target, Type dependencyType, Scope scope, Exception innerException)
            : base(String.Format(MessageTemplate,
                                 dependencyType.Name,
                                 target.GetType().Name,
                                 target.Name,
                                 scope.GetType().Name),
                   innerException)
        {
        }
    }
}