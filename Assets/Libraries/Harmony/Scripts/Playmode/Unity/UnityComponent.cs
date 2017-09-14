using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Component Unity.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    [NotTested(Reason.Wrapper)]
    public abstract class UnityComponent : IComponent
    {
        private readonly Component component;

        protected UnityComponent(Component component)
        {
            this.component = component;
        }

        // ReSharper disable once InconsistentNaming
        public GameObject gameObject
        {
            get { return component.gameObject; }
        }
    }
}