using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Disableable Unity.
    /// </summary>
    /// <inheritdoc cref="IDisableable"/>
    [NotTested(Reason.Wrapper)]
    public abstract class UnityDisableable : UnityComponent, IDisableable
    {
        private readonly Behaviour behaviour;

        protected UnityDisableable(Behaviour behaviour) : base(behaviour)
        {
            this.behaviour = behaviour;
        }

        public bool Enabled
        {
            get { return behaviour.enabled; }
            set { behaviour.enabled = value; }
        }
    }
}