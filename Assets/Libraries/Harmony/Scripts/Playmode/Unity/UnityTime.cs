using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Time Unity.
    /// </summary>
    /// <inheritdoc cref="ITime"/>
    [NotTested(Reason.Wrapper)]
    [AddComponentMenu("Game/Utils/UnityTime")]
    public class UnityTime : ITime
    {
        private float previousTimeScale;

        public float DeltaTime
        {
            get { return Time.deltaTime; }
        }

        public float FixedDeltaTime
        {
            get { return Time.fixedDeltaTime; }
        }

        public void Resume()
        {
            if (IsPaused())
            {
                Time.timeScale = previousTimeScale;
            }
        }

        public void Pause()
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public bool IsPaused()
        {
            return Time.timeScale == 0;
        }
    }
}