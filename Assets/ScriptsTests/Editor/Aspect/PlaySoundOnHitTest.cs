using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class PlaySoundOnHitTest : UnitTestCase
    {
        private const int HitPoints = 10;

        private AudioClip audioClip;
        private IAudioSource audioSource;
        private HitSensor hitSensor;
        private PlaySoundOnHit playSoundOnHit;

        [SetUp]
        public void Before()
        {
            audioClip = new AudioClip();
            audioSource = CreateSubstitute<IAudioSource>();
            hitSensor = CreateSubstitute<HitSensor>();
            playSoundOnHit = CreateBehaviour<PlaySoundOnHit>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            hitSensor.Received().OnHit += Arg.Any<HitSensorEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            hitSensor.Received().OnHit -= Arg.Any<HitSensorEventHandler>();
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
            playSoundOnHit.InjectPlaySoundOnHit(audioClip, audioSource, hitSensor);
            playSoundOnHit.Awake();
            playSoundOnHit.OnEnable();
        }

        private void Disable()
        {
            playSoundOnHit.OnDisable();
        }

        private void HitHitbox()
        {
            hitSensor.OnHit += Raise.Event<HitSensorEventHandler>(HitPoints);
        }

        private void CheckPlayedAudioClip()
        {
            audioSource.Received().PlayOneShot(audioClip);
        }
    }
}