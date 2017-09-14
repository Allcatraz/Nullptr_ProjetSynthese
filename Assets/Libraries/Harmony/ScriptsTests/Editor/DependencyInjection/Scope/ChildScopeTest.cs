using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Harmony.Injection
{
    public class ChildScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private ChildScope childScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            childScope = new ChildScope();
        }

        [Test]
        public void ReturnsAllChildrensOfGameObject()
        {
            GameObject parentGameObject = CreateGameObject("Parent");
            GameObject targetGameObject = CreateGameObject("Target");
            GameObject children1GameObject = CreateGameObject("Children1");
            GameObject children2GameObject = CreateGameObject("Children2");
            GameObject subChildren1GameObject = CreateGameObject("SubChildren1");
            GameObject subChildren2GameObject = CreateGameObject("SubChildren2");
            targetGameObject.transform.parent = parentGameObject.transform;
            children1GameObject.transform.parent = targetGameObject.transform;
            children2GameObject.transform.parent = targetGameObject.transform;
            subChildren1GameObject.transform.parent = children1GameObject.transform;
            subChildren2GameObject.transform.parent = children1GameObject.transform;
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();

            IList<object> actualDependencies = childScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(children1GameObject, actualDependencies[0]);
            Assert.AreSame(subChildren1GameObject, actualDependencies[1]);
            Assert.AreSame(subChildren2GameObject, actualDependencies[2]);
            Assert.AreSame(children2GameObject, actualDependencies[3]);
        }

        [Test]
        public void ReturnsAllDependenciesInChildrensOfGameObject()
        {
            GameObject parentGameObject = CreateGameObject("Parent");
            GameObject targetGameObject = CreateGameObject("Target");
            GameObject children1GameObject = CreateGameObject("Children1");
            GameObject children2GameObject = CreateGameObject("Children2");
            GameObject subChildren1GameObject = CreateGameObject("SubChildren1");
            GameObject subChildren2GameObject = CreateGameObject("SubChildren2");
            targetGameObject.transform.parent = parentGameObject.transform;
            children1GameObject.transform.parent = targetGameObject.transform;
            children2GameObject.transform.parent = targetGameObject.transform;
            subChildren1GameObject.transform.parent = children1GameObject.transform;
            subChildren2GameObject.transform.parent = children1GameObject.transform;
            EmptyBehaviour target = targetGameObject.AddComponent<EmptyBehaviour>();
            parentGameObject.AddComponent<OtherEmptyBehaviour>();
            targetGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour children1Dependency = children1GameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour children2Dependency = children2GameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour subChildren1Dependency = subChildren1GameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour subChildren2Dependency = subChildren2GameObject.AddComponent<OtherEmptyBehaviour>();

            IList<object> actualDependencies = childScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(children1Dependency, actualDependencies[0]);
            Assert.AreSame(subChildren1Dependency, actualDependencies[1]);
            Assert.AreSame(subChildren2Dependency, actualDependencies[2]);
            Assert.AreSame(children2Dependency, actualDependencies[3]);
        }
    }
}