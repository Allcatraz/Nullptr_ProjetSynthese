using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;

namespace Harmony.Injection
{
    public class ApplicationScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private EmptyBehaviour target;
        private ApplicationScope applicatonScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            target = CreateBehaviour<EmptyBehaviour>();
            applicatonScope = new ApplicationScope();
        }

        [Test]
        public void ThrowsExceptionIfNoTaggedObjectExists()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ApplicationDependencies).Returns(new List<GameObject>());

            Assert.Throws<DependencySourceNotFoundException>(
                delegate { applicatonScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour)); });
        }

        [Test]
        public void ThrowsExceptionIfMoreThanOneObjectHasTheTag()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ApplicationDependencies).Returns(new List<GameObject>(new[]
            {
                CreateGameObject(), CreateGameObject()
            }));

            Assert.Throws<MoreThanOneDependencySourceFoundException>(
                delegate { applicatonScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour)); });
        }

        [Test]
        public void ReturnsStaticDependenciesIfAskedTo()
        {
            IComponent staticDependency = CreateSubstitute<IComponent>();
            injectionContext.StaticComponents.Returns(new[] {staticDependency});

            IList<object> actualDependencies = applicatonScope.GetDependencies(injectionContext, target, typeof(IComponent));

            Assert.AreSame(staticDependency, actualDependencies[0]);
        }

        [Test]
        public void ReturnDependenciesOnTheSameLevelAndChildrens()
        {
            GameObject applicationGameObject = CreateGameObject();
            GameObject childGameObject = CreateGameObject();
            childGameObject.transform.parent = applicationGameObject.transform;
            OtherEmptyBehaviour sameLevelDependency = applicationGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour childrenDependency = childGameObject.AddComponent<OtherEmptyBehaviour>();
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ApplicationDependencies).Returns(new List<GameObject>(new[]
            {
                applicationGameObject
            }));

            IList<object> actualDependencies = applicatonScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(sameLevelDependency, actualDependencies[0]);
            Assert.AreSame(childrenDependency, actualDependencies[1]);
        }

        [Test]
        public void ReturnsGameObjectAndChildrens()
        {
            GameObject applicationGameObject = CreateGameObject();
            GameObject childrenGameObject = CreateGameObject();
            childrenGameObject.transform.parent = applicationGameObject.transform;
            injectionContext.FindGameObjectsWithTag(R.S.Tag.ApplicationDependencies).Returns(new List<GameObject>(new[]
            {
                applicationGameObject
            }));

            IList<object> actualDependencies = applicatonScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(applicationGameObject, actualDependencies[0]);
            Assert.AreSame(childrenGameObject, actualDependencies[1]);
        }
    }
}