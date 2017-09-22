using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/PlaySoundOnHitStimulus")]
    public class PlaySoundOnHitStimulus : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private AudioSource audioSource;
        private HitStimulus hitStimulus;

        private void InjectPlaySoundOnHitStimulus([EntityScope] AudioSource audioSource,
                                                 [GameObjectScope] HitStimulus hitStimulus)
        {
            this.audioSource = audioSource;
            this.hitStimulus = hitStimulus;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlaySoundOnHitStimulus");
        }

        private void OnEnable()
        {
            hitStimulus.OnHit += OnHit;
        }

        private void OnDisable()
        {
            hitStimulus.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}