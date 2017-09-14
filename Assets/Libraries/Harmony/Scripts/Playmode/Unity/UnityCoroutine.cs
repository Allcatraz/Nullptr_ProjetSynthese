using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Reprsente une Couroutine Unity.
    /// </summary>
    /// <inheritdoc cref="ICoroutine"/>
    [NotTested(Reason.Wrapper)]
    public class UnityCoroutine : ICoroutine
    {
        private readonly UnityScript target;
        private readonly Coroutine coroutine;

        public UnityCoroutine(UnityScript target, Coroutine coroutine)
        {
            this.target = target;
            this.coroutine = coroutine;
        }

        public void Stop()
        {
            target.StopCoroutine(coroutine);
        }
    }
}