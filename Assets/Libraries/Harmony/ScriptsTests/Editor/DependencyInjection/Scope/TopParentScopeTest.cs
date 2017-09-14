using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;

namespace Harmony.Injection
{
    public class TopParentScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private TopParentScope topParentScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            topParentScope = new TopParentScope();
        }

        [Test]
        public void ReturnDependenciesInUppermostParent()
        {
            GameObject parentGameObject = new GameObject();
            GameObject targetGameObject = new GameObject();
            targetGameObject.transform.parent = parentGameObject.transform;
            OtherEmptyBehaviour dependency = parentGameObject.AddComponent<OtherEmptyBehaviour>();
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();

            IList<object> actualDependencies = topParentScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(dependency, actualDependencies[0]);
        }

        [Test]
        public void ReturnDependenciesInSameGameObjectIfIsUppermostParent()
        {
            GameObject targetGameObject = new GameObject();
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();
            OtherEmptyBehaviour dependency = targetGameObject.AddComponent<OtherEmptyBehaviour>();

            IList<object> actualDependencies = topParentScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(dependency, actualDependencies[0]);
        }

        [Test]
        public void ReturnsTopParent()
        {
            GameObject parentGameObject = CreateGameObject();
            GameObject targetGameObject = CreateGameObject();
            targetGameObject.transform.parent = parentGameObject.transform;
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();

            IList<object> actualDependencies = topParentScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(parentGameObject, actualDependencies[0]);
        }
    }
}