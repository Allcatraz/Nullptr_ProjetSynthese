using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente une AudioSource Unity.
    /// </summary>
    /// <inheritdoc cref="IAudioSource"/>
    [NotTested(Reason.Wrapper)]
    public class UnityAudioSource : UnityDisableable, IAudioSource
    {
        private readonly AudioSource audioSource;

        public UnityAudioSource(AudioSource audioSource) : base(audioSource)
        {
            this.audioSource = audioSource;
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (audioSource.isActiveAndEnabled)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }
}