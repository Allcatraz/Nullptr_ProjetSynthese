using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Harmony.Injection
{
    /// <summary>
    /// Signature des fonctions servant à créer un Wrapper autour d'un objet.
    /// </summary>
    /// <param name="dependency">Objet à wrapper.</param>
    /// <returns>Wrapper autour de l'objet fourni.</returns>
    public delegate object WrapperFactoryCallback(object dependency);

    /// <summary>
    /// Représente une fabrique de Wrappers.
    /// </summary>
    /// <remarks>
    /// Un Wrapper est une sorte de pont vers un autre objet. Il permet de faire l'abstraction et la communication entre deux
    /// couches incompatibles directement.
    /// </remarks>
    public class WrapperFactory
    {
        private readonly Type wrappedType;
        private readonly Type wrapperType;
        private readonly WrapperFactoryCallback wrapperFactoryCallback;

        /// <summary>
        /// Construit une nouvelle fabrique de Wrapper pour les types donnés.
        /// </summary>
        /// <param name="wrappedType">Type des objets à wrapper.</param>
        /// <param name="wrapperType">Type du wrapper. Doit absolument être une abstraction, soit une interface.</param>
        /// <param name="wrapperFactoryCallback"></param>
        public WrapperFactory([NotNull] Type wrappedType, [NotNull] Type wrapperType, [NotNull] WrapperFactoryCallback wrapperFactoryCallback)
        {
            this.wrappedType = wrappedType;
            this.wrapperType = wrapperType;
            this.wrapperFactoryCallback = wrapperFactoryCallback;

            if (!wrapperType.IsInterface)
            {
                throw new ArgumentException("The WrapperType must be an interface, but the WrapperFactoryCallback can (and must) " +
                                            "return a specific implementation of this interface.");
            }
        }

        /// <summary>
        /// Indique si cette fabrique peut créer un Wrapper autour d'un objet du type spécifié, en prenant compte l'héritage.
        /// </summary>
        /// <param name="type">Type qui doit être wrappé.</param>
        /// <returns>Vrai si cette fabrique peut créer un Wrapper autour d'un objet de ce type, faux sinon.</returns>
        public bool CanWrap([NotNull] Type type)
        {
            return type == wrappedType || wrappedType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Indique si cette fabrique créé des Wrappers du type spécifié, ignorant l'héritage.
        /// </summary>
        /// <param name="type">Type de Wrapper.</param>
        /// <returns>Vrai si la fabrique peut créer des Wrappers de ce type, faux sinon.</returns>
        public bool CanWrapInto([NotNull] Type type)
        {
            return type == wrapperType;
        }

        /// <summary>
        /// Indique si le type fourni est une implémentation du type de Wrapper fabriqué par cette fabrique.
        /// </summary>
        /// <param name="type">Type de Wrapper.</param>
        /// <returns>Vrai si le type fourni est une implémentation du type de Wrapper fabriqué par cette fabrique, faux sinon</returns>
        public bool IsWrapperTypeImplementation([NotNull] Type type)
        {
            return type != wrapperType && wrapperType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Retourne le type wrappé par les Wrappers créée par cette fabrique.
        /// </summary>
        /// <returns>Le type wrappé.</returns>
        [NotNull]
        public Type GetWrappedType()
        {
            return wrappedType;
        }

        /// <summary>
        /// Retourne le type de Wrapper fabriqué par cette fabrique.
        /// </summary>
        /// <returns>Le type de Wrapper.</returns>
        [NotNull]
        public Type GetWrapperType()
        {
            return wrapperType;
        }

        /// <summary>
        /// Wrap l'objet fourni.
        /// </summary>
        /// <param name="wrappedObject">Objet à wrapper.</param>
        /// <returns>Nouveau wrapper autour de l'objet fourni.</returns>
        [NotNull]
        public object Wrap([NotNull] object wrappedObject)
        {
            return wrapperFactoryCallback(wrappedObject);
        }

        /// <summary>
        /// Wrap les objets fournis.
        /// </summary>
        /// <param name="wrappedObjects">Liste d'objet à wrapper.</param>
        /// <returns>Nouveaux wrappers autour des objets fournis.</returns>
        [NotNull]
        public IList<object> Wrap([NotNull] IList<object> wrappedObjects)
        {
            IList<object> wrappedDependencies = new List<object>();
            foreach (object wrappedObject in wrappedObjects)
            {
                wrappedDependencies.Add(Wrap(wrappedObject));
            }
            return wrappedDependencies;
        }
    }
}