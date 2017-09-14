using System;
using System.Collections.Generic;
using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Harmony.Injection
{
    public class ScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private EmptyBehaviour target;
        private ScopeForTest scope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(
                new List<WrapperFactory>
                {
                    new WrapperFactory(typeof(ComponentForTest),
                                       typeof(IComponentWrapperForTest),
                                       dependency => new ComponentWrapperForTest(dependency as ComponentForTest))
                });
            scope = new ScopeForTest();
            target = CreateBehaviour<EmptyBehaviour>();
        }

        [Test]
        public void ReturnsGameObjectsFromChildClasses()
        {
            GameObject[] expectedGameObjects = {CreateGameObject(), CreateGameObject()};
            scope.gameObjectsToReturn = expectedGameObjects;

            IList<object> actualDependencies = scope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreEqual(expectedGameObjects, actualDependencies);
        }

        [Test]
        public void ReturnsDependenciesFromChildClasses()
        {
            object[] expectedDependencies = {CreateSubstitute<object>(), CreateSubstitute<object>()};
            scope.dependenciesToReturn = expectedDependencies;

            IList<object> actualDependencies = scope.GetDependencies(injectionContext, target, typeof(Component));

            Assert.AreEqual(expectedDependencies, actualDependencies);
        }

        [Test]
        public void ReturnsWrapperInsteadOfComponentWhenItCan()
        {
            object[] expectedDependencies = {CreateSubstitute<ComponentForTest>()};
            scope.dependenciesToReturn = expectedDependencies;

            IList<object> actualDependencies = scope.GetDependencies(injectionContext, target, typeof(IComponentWrapperForTest));

            Assert.IsInstanceOf<ComponentWrapperForTest>(actualDependencies[0]);
        }

        [Test]
        public void ThrowsExceptionIfWrapperWasNotAskedWhenComponentTypeCanBeWrapped()
        {
            object[] expectedDependencies = {CreateSubstitute<ComponentForTest>()};
            scope.dependenciesToReturn = expectedDependencies;

            Assert.Throws<WrapperNotUsedException>(delegate { scope.GetDependencies(injectionContext, target, typeof(ComponentForTest)); });
        }

        [Test]
        public void ThrowsExceptionIfWrapperWasNotAskedWhenComponentSubTypeCanBeWrapped()
        {
            object[] expectedDependencies = {CreateSubstitute<ComponentForTest>()};
            scope.dependenciesToReturn = expectedDependencies;

            Assert.Throws<WrapperNotUsedException>(delegate { scope.GetDependencies(injectionContext, target, typeof(SubComponentForTest)); });
        }

        [Test]
        public void ThrowsExceptionIfWrapperTypeWasNotAskedDirectlyWhenComponentTypeCanBeWrapped()
        {
            object[] expectedDependencies = {CreateSubstitute<ComponentForTest>()};
            scope.dependenciesToReturn = expectedDependencies;

            Assert.Throws<WrongWrapperUsedException>(delegate { scope.GetDependencies(injectionContext, target, typeof(ComponentWrapperForTest)); });
        }

        [Test]
        public void ThrowsExceptionIfWrapperTypeWasNotAskedDirectlyWhenComponentSubTypeCanBeWrapped()
        {
            object[] expectedDependencies = {CreateSubstitute<ComponentForTest>()};
            scope.dependenciesToReturn = expectedDependencies;

            Assert.Throws<WrongWrapperUsedException>(delegate { scope.GetDependencies(injectionContext, target, typeof(SubComponentWrapperForTest)); });
        }

        private class ScopeForTest : Scope
        {
            public GameObject[] gameObjectsToReturn = {};
            public object[] dependenciesToReturn = {};

            protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
            {
                return gameObjectsToReturn;
            }

            protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
            {
                return dependenciesToReturn;
            }
        }

        public interface IComponentWrapperForTest : IComponent
        {
        }

        public interface ISubComponentWrapperForTest : IComponentWrapperForTest
        {
        }

        public class ComponentForTest : Component
        {
        }

        public class SubComponentForTest : ComponentForTest
        {
        }

        public class ComponentWrapperForTest : UnityComponent, IComponentWrapperForTest
        {
            public ComponentWrapperForTest(ComponentForTest component) : base(component)
            {
            }
        }

        public class SubComponentWrapperForTest : UnityComponent, ISubComponentWrapperForTest
        {
            public SubComponentWrapperForTest(SubComponentForTest component) : base(component)
            {
            }
        }
    }
}