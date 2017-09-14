using Harmony;
using Harmony.EventSystem;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class PrefabInstanceCollectionTest : UnitTestCase
    {
        private const R.E.Prefab Prefab = R.E.Prefab.Player;
        private const R.E.Prefab WrongPrefab = R.E.Prefab.Rock;

        private CreationEventChannel creationEventChannel;
        private DestroyEventChannel destroyEventChannel;
        private IHierachy hierachy;
        private PrefabInstanceCollection prefabInstanceCollection;

        [SetUp]
        public void Before()
        {
            creationEventChannel = CreateSubstitute<CreationEventChannel>();
            destroyEventChannel = CreateSubstitute<DestroyEventChannel>();
            hierachy = CreateSubstitute<IHierachy>();
            prefabInstanceCollection = CreateBehaviour<PrefabInstanceCollection>();
        }

        [Test]
        public void WhenCreatedRegistersToEvents()
        {
            Initialize();

            creationEventChannel.Received().OnEventPublished += Arg.Any<EventChannelHandler<CreationEvent>>();
            destroyEventChannel.Received().OnEventPublished += Arg.Any<EventChannelHandler<DestroyEvent>>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            creationEventChannel.Received().OnEventPublished -= Arg.Any<EventChannelHandler<CreationEvent>>();
            destroyEventChannel.Received().OnEventPublished -= Arg.Any<EventChannelHandler<DestroyEvent>>();
        }

        [Test]
        public void OnPrefabCreationAddItToCollection()
        {
            Initialize();

            CreatePrefab();
            CreateWrongPrefab();

            CheckPrefabAdded(1);
        }

        [Test]
        public void CanGetCountOfPrefabs()
        {
            Initialize();

            CreatePrefab();
            CreateWrongPrefab();

            CheckCount(1);
        }

        [Test]
        public void CanDestroyAllPrefabs()
        {
            Initialize();

            GameObject prefab1 = CreatePrefab();
            GameObject prefab2 = CreatePrefab();
            GameObject prefab3 = CreatePrefab();
            prefabInstanceCollection.DestroyAll();

            CheckCount(0);
            CheckDestroyed(prefab1);
            CheckDestroyed(prefab2);
            CheckDestroyed(prefab3);
        }

        private void Initialize()
        {
            prefabInstanceCollection.InjectPrefabInstanceCollection(Prefab,
                                                                    hierachy,
                                                                    creationEventChannel,
                                                                    destroyEventChannel);
            prefabInstanceCollection.Awake();
        }

        private void Destroy()
        {
            prefabInstanceCollection.OnDestroy();
        }

        private GameObject CreatePrefab()
        {
            GameObject createdGameObject = CreateGameObject();
            creationEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<CreationEvent>>(new CreationEvent(Prefab,
                                                                                                                       createdGameObject));
            return createdGameObject;
        }

        private GameObject CreateWrongPrefab()
        {
            GameObject createdGameObject = CreateGameObject();
            creationEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<CreationEvent>>(new CreationEvent(WrongPrefab,
                                                                                                                       createdGameObject));
            return createdGameObject;
        }

        private void CheckPrefabAdded(int numberOfElements)
        {
            Assert.AreEqual(numberOfElements, prefabInstanceCollection.Count);
        }

        private void CheckCount(int numberOfElements)
        {
            Assert.AreEqual(numberOfElements, prefabInstanceCollection.Count);
        }

        private void CheckDestroyed(GameObject prefab)
        {
            hierachy.Received().DestroyGameObject(prefab);
        }
    }
}