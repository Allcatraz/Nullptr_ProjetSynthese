using System.Collections.Generic;
using Harmony.Testing;
using Harmony.Unity;
using NUnit.Framework;
using UnityEngine;

namespace Harmony.Injection
{
    public class NamedTest : UnitTestCase
    {
        private const string GoodName = "GOOD_NAME";
        private const string WrongName = "WRONG_NAME";

        private Named named;

        [SetUp]
        public void Before()
        {
            named = new Named(GoodName);
        }

        [Test]
        public void ReturnsDependencyWithinGameObjectOfProvidedName()
        {
            GameObject goodGameObject = CreateGameObject(GoodName);
            EmptyBehaviour goodDependency = goodGameObject.AddComponent<EmptyBehaviour>();
            GameObject wrongGameObject = CreateGameObject(WrongName);
            EmptyBehaviour wrongDependency = wrongGameObject.AddComponent<EmptyBehaviour>();

            IList<object> dependencies = new object[] {goodDependency, wrongDependency};

            IList<object> filteredDependencies = named.FilterDependencies(dependencies);

            Assert.AreEqual(1, filteredDependencies.Count);
            Assert.AreSame(goodDependency, filteredDependencies[0]);
        }

        [Test]
        public void ReturnsUnityWrapperWithinGameObjectOfProvidedName()
        {
            GameObject goodGameObject = CreateGameObject(GoodName);
            EmptyBehaviour goodDependency = goodGameObject.AddComponent<EmptyBehaviour>();
            UnityComponentForTest goodWrapper = new UnityComponentForTest(goodDependency);
            GameObject wrongGameObject = CreateGameObject(WrongName);
            EmptyBehaviour wrongDependency = wrongGameObject.AddComponent<EmptyBehaviour>();
            UnityComponentForTest wrongWrapper = new UnityComponentForTest(wrongDependency);

            IList<object> dependencies = new object[] {goodWrapper, wrongWrapper};

            IList<object> filteredDependencies = named.FilterDependencies(dependencies);

            Assert.AreEqual(1, filteredDependencies.Count);
            Assert.AreSame(goodWrapper, filteredDependencies[0]);
        }

        [Test]
        public void ReturnsGameObjectOfProvidedName()
        {
            GameObject goodGameObject = CreateGameObject(GoodName);
            GameObject wrongGameObject = CreateGameObject(WrongName);
            IList<object> gameObjects = new object[] {goodGameObject, wrongGameObject};

            IList<object> filteredGameObjects = named.FilterDependencies(gameObjects);

            Assert.AreEqual(1, filteredGameObjects.Count);
            Assert.AreSame(goodGameObject, filteredGameObjects[0]);
        }

        private class UnityComponentForTest : UnityComponent
        {
            public UnityComponentForTest(EmptyBehaviour behaviour) : base(behaviour)
            {
            }
        }
    }
}