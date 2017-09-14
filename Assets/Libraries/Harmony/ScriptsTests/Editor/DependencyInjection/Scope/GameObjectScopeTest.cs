using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;

namespace Harmony.Injection
{
    public class GameObjectScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private GameObjectScope gameObjectScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            gameObjectScope = new GameObjectScope();
        }

        [Test]
        public void ReturnDependenciesOnSameGameObject()
        {
            GameObject targetGameObject = new GameObject();
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();
            OtherEmptyBehaviour dependency = targetGameObject.AddComponent<OtherEmptyBehaviour>();

            IList<object> actualDependencies = gameObjectScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));
            Assert.AreSame(dependency, actualDependencies[0]);
        }

        [Test]
        public void WhenAskedForGameObjectReturnsItself()
        {
            GameObject targetGameObject = CreateGameObject();
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();

            IList<object> actualDependencies = gameObjectScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(targetGameObject, actualDependencies[0]);
        }
    }
}