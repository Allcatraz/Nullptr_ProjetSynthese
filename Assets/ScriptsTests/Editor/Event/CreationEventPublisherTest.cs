using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class CreationEventPublisherTest : UnitTestCase
    {
        private const R.E.Prefab CreatedPrefab = R.E.Prefab.Player;

        private GameObject topParent;
        private CreationEventChannel eventChannel;
        private CreationEventPublisher eventPublisher;

        [SetUp]
        public void Before()
        {
            topParent = CreateGameObject();
            eventChannel = CreateSubstitute<CreationEventChannel>();
            eventPublisher = CreateBehaviour<CreationEventPublisher>();
        }

        [Test]
        public void PublishEventOnAwake()
        {
            Initialize();

            CheckEventPublished();
        }

        private void Initialize()
        {
            eventPublisher.InjectCreationEventPublisher(CreatedPrefab, topParent, eventChannel);
            eventPublisher.Awake();
        }

        private void CheckEventPublished()
        {
            eventChannel.Received().Publish(Arg.Is<CreationEvent>(creationEvent =>
                                                                      creationEvent.CreatedGameObject == topParent &&
                                                                      creationEvent.CreatedPrefab == CreatedPrefab));
        }
    }
}