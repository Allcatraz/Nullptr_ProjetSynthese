using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class EntityDestroyerTest : UnitTestCase
    {
        private GameObject topParent;
        private IHierachy hierachy;
        private EntityDestroyer entityDestroyer;

        [SetUp]
        public void Before()
        {
            topParent = CreateGameObject();
            hierachy = CreateSubstitute<IHierachy>();
            entityDestroyer = CreateBehaviour<EntityDestroyer>();
        }

        [Test]
        public void CanDestroyEntityByDestroyingTopParent()
        {
            Initialize();

            entityDestroyer.Destroy();

            CheckTopParentDestroyed();
        }

        private void Initialize()
        {
            entityDestroyer.InjectEntityDestroyer(topParent, hierachy);
            entityDestroyer.Awake();
        }

        private void CheckTopParentDestroyed()
        {
            hierachy.Received().DestroyGameObject(topParent);
        }
    }
}