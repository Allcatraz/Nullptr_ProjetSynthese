using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;

namespace Harmony.Injection
{
    public class TagScopeTest : UnitTestCase
    {
        private const string Tag = R.S.Tag.Player; //We use real tags that exists, because Unity want it.

        private IInjectionContext injectionContext;
        private EmptyBehaviour target;
        private TagScope tagScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            target = CreateBehaviour<EmptyBehaviour>();
            tagScope = new TagScope(Tag);
        }

        [Test]
        public void ThrowsExceptionIfNoTaggedObjectExists()
        {
            injectionContext.FindGameObjectsWithTag(Tag).Returns(new List<GameObject>());

            Assert.Throws<DependencySourceNotFoundException>(delegate
            {
                tagScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));
            });
        }

        [Test]
        public void ReturnDependenciesOnTheSameLevelAndChildrens()
        {
            GameObject tagGameObject = CreateGameObject();
            GameObject childGameObject = CreateGameObject();
            childGameObject.transform.parent = tagGameObject.transform;
            OtherEmptyBehaviour sameLevelDependency = tagGameObject.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour childrenDependency = childGameObject.AddComponent<OtherEmptyBehaviour>();
            injectionContext.FindGameObjectsWithTag(Tag).Returns(new List<GameObject>(new[]
            {
                tagGameObject
            }));

            IList<object> actualDependencies = tagScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(sameLevelDependency, actualDependencies[0]);
            Assert.AreSame(childrenDependency, actualDependencies[1]);
        }

        [Test]
        public void ReturnDependenciesOnMultipleTaggedObject()
        {
            GameObject tagGameObject1 = CreateGameObject();
            GameObject tagGameObject2 = CreateGameObject();
            OtherEmptyBehaviour dependency1 = tagGameObject1.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour dependency2 = tagGameObject2.AddComponent<OtherEmptyBehaviour>();
            injectionContext.FindGameObjectsWithTag(Tag).Returns(new List<GameObject>(new[]
            {
                tagGameObject1, tagGameObject2
            }));

            IList<object> actualDependencies = tagScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(dependency1, actualDependencies[0]);
            Assert.AreSame(dependency2, actualDependencies[1]);
        }

        [Test]
        public void ReturnsGameObjectWithTag()
        {
            GameObject tagGameObject = CreateGameObject();
            injectionContext.FindGameObjectsWithTag(Tag).Returns(new List<GameObject>(new[]
            {
                tagGameObject
            }));

            IList<object> actualDependencies = tagScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(tagGameObject, actualDependencies[0]);
        }
    }
}