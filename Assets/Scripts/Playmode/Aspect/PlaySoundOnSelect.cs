using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    //BEN_REVIEW : Ne semble pas servir nulle part
    
    [AddComponentMenu("Game/Aspect/PlaySoundOnSelect")]
    public class PlaySoundOnSelect : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private AudioSource audioSource;
        private Selectable selectable;

        private void InjectPlaySoundOnSelect([EntityScope] AudioSource audioSource,
                                            [GameObjectScope] Selectable selectable)
        {
            this.audioSource = audioSource;
            this.selectable = selectable;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlaySoundOnSelect");
        }

        private void OnEnable()
        {
            selectable.Events().OnSelected += OnSelected;
        }

        private void OnDisable()
        {
            selectable.Events().OnSelected += OnSelected;
        }

        private void OnSelected()
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}