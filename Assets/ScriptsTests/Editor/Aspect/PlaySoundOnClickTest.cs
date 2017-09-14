using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class PlaySoundOnClickTest : UnitTestCase
    {
        private AudioClip audioClip;
        private IAudioSource audioSource;
        private IButton button;
        private PlaySoundOnClick playSoundOnClick;

        [SetUp]
        public void Before()
        {
            audioClip = new AudioClip();
            audioSource = CreateSubstitute<IAudioSource>();
            button = CreateSubstitute<IButton>();
            playSoundOnClick = CreateBehaviour<PlaySoundOnClick>();
        }

        [Test]
        public void WhenStartingRegistersToEvents()
        {
            Initialize();

            button.Received().OnClicked += Arg.Any<ButtonClickedEventHandler>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            button.Received().OnClicked -= Arg.Any<ButtonClickedEventHandler>();
        }

        [Test]
        public void OnClickPlayProvidedAudioClip()
        {
            Initialize();

            ClickButton();

            CheckPlayedAudioClip();
        }

        private void Initialize()
        {
            playSoundOnClick.InjectPlaySoundOnClick(audioClip, audioSource, button);
            playSoundOnClick.Awake();
            playSoundOnClick.Start();
        }

        private void Destroy()
        {
            playSoundOnClick.OnDestroy();
        }

        private void ClickButton()
        {
            button.OnClicked += Raise.Event<ButtonClickedEventHandler>();
        }

        private void CheckPlayedAudioClip()
        {
            audioSource.Received().PlayOneShot(audioClip);
        }
    }
}