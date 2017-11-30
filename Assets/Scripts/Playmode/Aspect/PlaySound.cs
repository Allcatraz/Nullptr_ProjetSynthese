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

        public void Use()
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
