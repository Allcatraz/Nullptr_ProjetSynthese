using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/Aspect/PlaySoundOnSelect")]
    public class PlaySoundOnSelect : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private IAudioSource audioSource;
        private ISelectable selectable;

        public void InjectPlaySoundOnSelect(AudioClip audioClip,
                                            [EntityScope] IAudioSource audioSource,
                                            [GameObjectScope] ISelectable selectable)
        {
            this.audioClip = audioClip;
            this.audioSource = audioSource;
            this.selectable = selectable;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlaySoundOnSelect", audioClip);
        }

        public void Start()
        {
            selectable.OnSelected += OnSelected;
        }

        public void OnDestroy()
        {
            selectable.OnSelected -= OnSelected;
        }

        private void OnSelected()
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}