using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class DestroyEventPublisherTest : UnitTestCase
    {
        private const R.E.Prefab DestroyedPrefab = R.E.Prefab.Player;

        private GameObject topParent;
        private EntityDestroyer entityDestroyer;
        private DestroyEventChannel eventChannel;
        private DestroyEventPublisher eventPublisher;

        [SetUp]
        public void Before()
        {
            topParent = CreateGameObject();
            entityDestroyer = CreateSubstitute<EntityDestroyer>();
            eventChannel = CreateSubstitute<DestroyEventChannel>();
            eventPublisher = CreateBehaviour<DestroyEventPublisher>();
        }

        [Test]
        public void WhenCreatedRegistersToEvents()
        {
            Initialize();

            entityDestroyer.Received().OnDestroyed += Arg.Any<EntityDestroyedEventHandler>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            entityDestroyer.Received().OnDestroyed -= Arg.Any<EntityDestroyedEventHandler>();
        }

        [Test]
        public void OnDestroyPublishEvent()
        {
            Initialize();

            DestroyEntity();

            CheckEventPublished();
        }

        private void Initialize()
        {
            eventPublisher.InjectDestroyEventPublisher(DestroyedPrefab, topParent, entityDestroyer, eventChannel);
            eventPublisher.Awake();
        }

        private void Destroy()
        {
            eventPublisher.OnDestroy();
        }

        private void DestroyEntity()
        {
            entityDestroyer.OnDestroyed += Raise.Event<EntityDestroyedEventHandler>();
        }

        private void CheckEventPublished()
        {
            eventChannel.Received().Publish(Arg.Is<DestroyEvent>(destroyEvent =>
                                                                     destroyEvent.DestroyedGameObject == topParent &&
                                                                     destroyEvent.DestroyedPrefab == DestroyedPrefab));
        }
    }
}