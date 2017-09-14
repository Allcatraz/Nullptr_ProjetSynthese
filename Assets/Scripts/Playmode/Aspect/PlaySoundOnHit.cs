using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/PlaySoundOnHit")]
    public class PlaySoundOnHit : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private IAudioSource audioSource;
        private HitSensor hitSensor;

        public void InjectPlaySoundOnHit(AudioClip audioClip,
                                         [EntityScope] IAudioSource audioSource,
                                         [GameObjectScope] HitSensor hitSensor)
        {
            this.audioClip = audioClip;
            this.audioSource = audioSource;
            this.hitSensor = hitSensor;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlaySoundOnHit", audioClip);
        }

        public void OnEnable()
        {
            hitSensor.OnHit += OnHit;
        }

        public void OnDisable()
        {
            hitSensor.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}