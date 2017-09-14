using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class PlaySoundOnSelectTest : UnitTestCase
    {
        private AudioClip audioClip;
        private IAudioSource audioSource;
        private ISelectable selectable;
        private PlaySoundOnSelect playSoundOnSelect;

        [SetUp]
        public void Before()
        {
            audioClip = new AudioClip();
            audioSource = CreateSubstitute<IAudioSource>();
            selectable = CreateSubstitute<ISelectable>();
            playSoundOnSelect = CreateBehaviour<PlaySoundOnSelect>();
        }

        [Test]
        public void WhenStartingRegistersToEvents()
        {
            Initialize();

            selectable.Received().OnSelected += Arg.Any<SelectedEventHandler>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            selectable.Received().OnSelected -= Arg.Any<SelectedEventHandler>();
        }

        [Test]
        public void OnSelectedPlayProvidedAudioClip()
        {
            Initialize();

            Select();

            CheckPlayedAudioClip();
        }

        private void Initialize()
        {
            playSoundOnSelect.InjectPlaySoundOnSelect(audioClip, audioSource, selectable);
            playSoundOnSelect.Awake();
            playSoundOnSelect.Start();
        }

        private void Destroy()
        {
            playSoundOnSelect.OnDestroy();
        }

        private void Select()
        {
            selectable.OnSelected += Raise.Event<SelectedEventHandler>();
        }

        private void CheckPlayedAudioClip()
        {
            audioSource.Received().PlayOneShot(audioClip);
        }
    }
}