using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/PlaySoundOnClick")]
    public class PlaySoundOnClick : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private AudioSource audioSource;
        private Button button;

        private void InjectPlaySoundOnClick([EntityScope] AudioSource audioSource,
                                           [GameObjectScope] Button button)
        {
            this.audioSource = audioSource;
            this.button = button;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlaySoundOnClick");
        }

        private void Start()
        {
            button.Events().OnClick += OnClicked;
        }

        private void OnDestroy()
        {
            button.Events().OnClick -= OnClicked;
        }

        private void OnClicked()
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}