using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Harmony.Injection
{
    public class ActivityScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private EmptyBehaviour target;
        private ActivityScope activityScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            target = CreateBehaviour<EmptyBehaviour>();
            activityScope = new ActivityScope();
        }

        [Test]
        public void ThrowsExceptionIfNoTaggedObjectExists()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ActivityDependencies).Returns(new List<GameObject>());

            Assert.Throws<DependencySourceNotFoundException>(
                delegate { activityScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour)); });
        }

        [Test]
        public void ThrowsExceptionIfMoreThanOneObjectHasTheTag()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ActivityDependencies).Returns(new List<GameObject>(new[]
            {
                CreateGameObject(), CreateGameObject()
            }));

            Assert.Throws<MoreThanOneDependencySourceFoundException>(
                delegate { activityScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour)); });
        }

        [Test]
        public void ReturnDependenciesOnTheSameLevelAndChildrens()
        {
            GameObject applicationGameObject = CreateGameObject();
            GameObject childGameObject = CreateGameObject();
            childGameObject.transform.parent = applicationGameObject.transform;
            OtherEmptyBehaviour sameLevelDependency = applicationGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour childrenDependency = childGameObject.AddComponent<OtherEmptyBehaviour>();
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ActivityDependencies).Returns(new List<GameObject>(new[]
            {
                applicationGameObject
            }));

            IList<object> actualDependencies = activityScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(sameLevelDependency, actualDependencies[0]);
            Assert.AreSame(childrenDependency, actualDependencies[1]);
        }

        [Test]
        public void ReturnsGameObjectAndChildrens()
        {
            GameObject applicationGameObject = CreateGameObject();
            GameObject childrenGameObject = CreateGameObject();
            childrenGameObject.transform.parent = applicationGameObject.transform;
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ActivityDependencies).Returns(new List<GameObject>(new[]
            {
                applicationGameObject
            }));

            IList<object> actualDependencies = activityScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(applicationGameObject, actualDependencies[0]);
            Assert.AreSame(childrenGameObject, actualDependencies[1]);
        }
    }
}