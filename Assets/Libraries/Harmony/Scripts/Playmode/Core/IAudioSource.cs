using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente une source sonore. En mesure de jouer des sons.
    /// </summary>
    public interface IAudioSource : IDisableable
    {
        /// <summary>
        /// Joue un son au complet une seule fois.
        /// </summary>
        /// <param name="audioClip">Le son à jouer.</param>
        void PlayOneShot([NotNull] AudioClip audioClip);
    }
}