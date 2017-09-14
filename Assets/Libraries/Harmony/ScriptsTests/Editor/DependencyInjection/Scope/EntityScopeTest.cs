using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;

namespace Harmony.Injection
{
    public class EntityScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private EntityScope entityScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            entityScope = new EntityScope();
        }

        [Test]
        public void ReturnDependenciesOnTheSameLevelSiblingsParentAndChildrens()
        {
            GameObject parentGameObject = CreateGameObject("Parent");
            GameObject targetGameObject = CreateGameObject("Target");
            GameObject siblingGameObject = CreateGameObject("Sibling");
            GameObject childrenGameObject = CreateGameObject("Children");
            targetGameObject.transform.parent = parentGameObject.transform;
            siblingGameObject.transform.parent = parentGameObject.transform;
            childrenGameObject.transform.parent = targetGameObject.transform;
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();
            OtherEmptyBehaviour parentDependency = parentGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour sameLevelDependency = targetGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour siblingDependency = siblingGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour childrenDependency = childrenGameObject.AddComponent<OtherEmptyBehaviour>();

            IList<object> actualDependencies = entityScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(parentDependency, actualDependencies[0]);
            Assert.AreSame(sameLevelDependency, actualDependencies[1]);
            Assert.AreSame(childrenDependency, actualDependencies[2]);
            Assert.AreSame(siblingDependency, actualDependencies[3]);
        }

        [Test]
        public void ReturnsChildrensOfTopParent()
        {
            GameObject parentGameObject = CreateGameObject();
            GameObject middleGameObject = CreateGameObject();
            GameObject targetGameObject = CreateGameObject();
            middleGameObject.transform.parent = parentGameObject.transform;
            targetGameObject.transform.parent = middleGameObject.transform;
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();

            IList<object> actualDependencies = entityScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(parentGameObject, actualDependencies[0]);
            Assert.AreSame(middleGameObject, actualDependencies[1]);
            Assert.AreSame(targetGameObject, actualDependencies[2]);
        }
    }
}