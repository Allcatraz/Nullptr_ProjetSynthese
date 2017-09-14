using System;
using System.Collections;
using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un exécuteur de Coroutine Unity.
    /// </summary>
    /// <inheritdoc cref="ICoroutineExecutor"/>
    [NotTested(Reason.Wrapper)]
    [AddComponentMenu("Game/Utils/UnityCoroutineExecutor")]
    public class UnityCoroutineExecutor : ICoroutineExecutor
    {
        public ICoroutine StartCoroutine(IScript target, IEnumerator coroutineMethod)
        {
            UnityScript unityScript = target as UnityScript;
            if (unityScript == null)
            {
                throw new ArgumentException("Target script is not a UnityScript. Please refrain from doing cross-framework usage of this class.");
            }
            return new UnityCoroutine(unityScript, unityScript.StartCoroutine(coroutineMethod));
        }
    }
}