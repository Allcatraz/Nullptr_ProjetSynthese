using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/PlaySoundOnHit")]
    public class PlaySoundOnHit : GameScript
    {
        [SerializeField]
        private AudioClip audioClip;

        private AudioSource audioSource;
        private HitSensor hitSensor;

        private void InjectPlaySoundOnHit([EntityScope] AudioSource audioSource,
                                         [GameObjectScope] HitSensor hitSensor)
        {
            this.audioSource = audioSource;
            this.hitSensor = hitSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlaySoundOnHit");
        }

        private void OnEnable()
        {
            hitSensor.OnHit += OnHit;
        }

        private void OnDisable()
        {
            hitSensor.OnHit -= OnHit;
        }

        private void OnHit(int hitPoints)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}