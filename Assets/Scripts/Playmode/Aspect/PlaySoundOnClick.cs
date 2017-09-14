using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Aspect/PlaySoundOnClick")]
    public class PlaySoundOnClick : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private IAudioSource audioSource;
        private IButton button;

        public void InjectPlaySoundOnClick(AudioClip audioClip,
                                           [EntityScope] IAudioSource audioSource,
                                           [GameObjectScope] IButton button)
        {
            this.audioClip = audioClip;
            this.audioSource = audioSource;
            this.button = button;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlaySoundOnClick", audioClip);
        }

        public void Start()
        {
            button.OnClicked += OnClicked;
        }

        public void OnDestroy()
        {
            button.OnClicked -= OnClicked;
        }

        private void OnClicked()
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}