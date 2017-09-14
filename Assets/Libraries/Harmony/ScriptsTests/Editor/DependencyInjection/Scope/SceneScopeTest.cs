using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;

namespace Harmony.Injection
{
    public class SceneScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private EmptyBehaviour target;
        private SceneScope sceneScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            target = CreateBehaviour<EmptyBehaviour>();
            sceneScope = new SceneScope();
        }

        [Test]
        public void ThrowsExceptionIfNoTaggedObjectExists()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.SceneDependencies).Returns(new List<GameObject>());

            Assert.Throws<DependencySourceNotFoundException>(delegate
            {
                sceneScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));
            });
        }

        [Test]
        public void ThrowsExceptionIfMoreThanOneObjectHasTheTag()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.SceneDependencies).Returns(new List<GameObject>(new[]
            {
                CreateGameObject(), CreateGameObject()
            }));

            Assert.Throws<MoreThanOneDependencySourceFoundException>(delegate
            {
                sceneScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));
            });
        }

        [Test]
        public void ReturnDependenciesOnTheSameLevelAndChildrens()
        {
            GameObject sceneGameObject = CreateGameObject();
            GameObject childGameObject = CreateGameObject();
            childGameObject.transform.parent = sceneGameObject.transform;
            OtherEmptyBehaviour sameLevelDependency = sceneGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour childrenDependency = childGameObject.AddComponent<OtherEmptyBehaviour>();
            injectionContext.FindGameObjectsWithTag(R.S.Tag.SceneDependencies).Returns(new List<GameObject>(new[]
            {
                sceneGameObject
            }));

            IList<object> actualDependencies = sceneScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(sameLevelDependency, actualDependencies[0]);
            Assert.AreSame(childrenDependency, actualDependencies[1]);
        }

        [Test]
        public void ReturnsGameObjectAndChildrens()
        {
            GameObject sceneGameObject = CreateGameObject();
            GameObject childrenGameObject = CreateGameObject();
            childrenGameObject.transform.parent = sceneGameObject.transform;
            injectionContext.FindGameObjectsWithTag(R.S.Tag.SceneDependencies).Returns(new List<GameObject>(new[]
            {
                sceneGameObject
            }));

            IList<object> actualDependencies = sceneScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(sceneGameObject, actualDependencies[0]);
            Assert.AreSame(childrenGameObject, actualDependencies[1]);
        }
    }
}