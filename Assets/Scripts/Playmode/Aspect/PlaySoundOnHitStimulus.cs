using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/PlaySoundOnHitStimulus")]
    public class PlaySoundOnHitStimulus : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private IAudioSource audioSource;
        private HitStimulus hitStimulus;

        public void InjectPlaySoundOnHitStimulus(AudioClip audioClip,
                                                 [EntityScope] IAudioSource audioSource,
                                                 [GameObjectScope] HitStimulus hitStimulus)
        {
            this.audioClip = audioClip;
            this.audioSource = audioSource;
            this.hitStimulus = hitStimulus;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlaySoundOnHitStimulus", audioClip);
        }

        public void OnEnable()
        {
            hitStimulus.OnHit += OnHit;
        }

        public void OnDisable()
        {
            hitStimulus.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}