using JetBrains.Annotations;
using NSubstitute;

namespace Harmony
{
    /// <summary>
    /// Représente un test unitaire. Contient quelques méthodes utilitaires pour simplifier les tests unitaires.
    /// </summary>
    public abstract class UnitTestCase
    {
        [NotNull]
        protected T CreateSubstitute<T>() where T : class
        {
            return Substitute.For<T>();
        }
    }
}