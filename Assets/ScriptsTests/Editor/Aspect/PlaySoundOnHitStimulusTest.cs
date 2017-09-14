using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class PlaySoundOnHitStimulusTest : UnitTestCase
    {
        private const int HitPoints = 10;

        private AudioClip audioClip;
        private IAudioSource audioSource;
        private HitStimulus hitStimulus;
        private PlaySoundOnHitStimulus playSoundOnHitStimulus;

        [SetUp]
        public void Before()
        {
            audioClip = new AudioClip();
            audioSource = CreateSubstitute<IAudioSource>();
            hitStimulus = CreateSubstitute<HitStimulus>();
            playSoundOnHitStimulus = CreateBehaviour<PlaySoundOnHitStimulus>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            hitStimulus.Received().OnHit += Arg.Any<HitStimulusEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            hitStimulus.Received().OnHit -= Arg.Any<HitStimulusEventHandler>();
        }

        [Test]
        public void OnHitPlayProvidedAudioClip()
        {
            Initialize();

            HitHitbox();

            CheckPlayedAudioClip();
        }

        private void Initialize()
        {
            playSoundOnHitStimulus.InjectPlaySoundOnHitStimulus(audioClip, audioSource, hitStimulus);
            playSoundOnHitStimulus.Awake();
            playSoundOnHitStimulus.OnEnable();
        }

        private void Disable()
        {
            playSoundOnHitStimulus.OnDisable();
        }

        private void HitHitbox()
        {
            hitStimulus.OnHit += Raise.Event<HitStimulusEventHandler>(HitPoints);
        }

        private void CheckPlayedAudioClip()
        {
            audioSource.Received().PlayOneShot(audioClip);
        }
    }
}