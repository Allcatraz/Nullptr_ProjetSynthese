using System.Collections;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class PlaySound : GameScript
    {
        [Tooltip("Sound that's going to be trigger.")]
        [SerializeField]
        AudioClip sound;

        private AudioSource audioSource;

        private void InjectPlaySound([EntityScope] AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlaySound");
        }

        public void Use(float delay)
        {
            StartCoroutine("DelayPlay", delay);
        }

        private IEnumerator DelayPlay(float delay)
        {
            bool needDelay = delay != 0;
            if(needDelay)
                yield return new WaitForSeconds(delay);
            audioSource.PlayOneShot(sound);
            yield return null;
        }
    }
}
