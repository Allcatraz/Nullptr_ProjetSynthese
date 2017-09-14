using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Harmony.Testing;
using NSubstitute;

namespace Harmony.Injection
{
    public class EventChannelScopeTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private EmptyBehaviour target;
        private EventChannelScope eventChannelScope;

        [SetUp]
        public void Before()
        {
            injectionContext = CreateSubstitute<IInjectionContext>();
            injectionContext.DependencyWrappers.Returns(new List<WrapperFactory>());
            target = CreateBehaviour<EmptyBehaviour>();
            eventChannelScope = new EventChannelScope();
        }

        [Test]
        public void ThrowsExceptionIfNoTaggedObjectExists()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.EventChannels).Returns(new List<GameObject>());

            Assert.Throws<DependencySourceNotFoundException>(delegate { eventChannelScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour)); });
        }

        [Test]
        public void DoNotThrowExceptionIfMoreThanOneObjectHasTheTag()
        {
            injectionContext.FindGameObjectsWithTag(R.S.Tag.EventChannels).Returns(new List<GameObject>(new[]
            {
                CreateGameObject(), CreateGameObject()
            }));

            eventChannelScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));
        }

        [Test]
        public void ReturnDependenciesOnTheSameLevelAndChildrens()
        {
            GameObject eventChannelGameObject1 = CreateGameObject();
            GameObject eventChannelGameObject2 = CreateGameObject();
            GameObject childGameObject1 = CreateGameObject();
            GameObject childGameObject2 = CreateGameObject();
            childGameObject1.transform.parent = eventChannelGameObject1.transform;
            childGameObject2.transform.parent = eventChannelGameObject2.transform;
            OtherEmptyBehaviour sameLevelDependency1 = eventChannelGameObject1.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour sameLevelDependency2 = eventChannelGameObject2.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour childrenDependency1 = childGameObject1.AddComponent<OtherEmptyBehaviour>();
            OtherEmptyBehaviour childrenDependency2 = childGameObject2.AddComponent<OtherEmptyBehaviour>();
            injectionContext.FindGameObjectsWithTag(R.S.Tag.EventChannels).Returns(new List<GameObject>(new[]
            {
                eventChannelGameObject1, eventChannelGameObject2
            }));

            IList<object> actualDependencies = eventChannelScope.GetDependencies(injectionContext, target, typeof(OtherEmptyBehaviour));

            Assert.AreSame(sameLevelDependency1, actualDependencies[0]);
            Assert.AreSame(childrenDependency1, actualDependencies[1]);
            Assert.AreSame(sameLevelDependency2, actualDependencies[2]);
            Assert.AreSame(childrenDependency2, actualDependencies[3]);
        }

        [Test]
        public void ReturnsGameObjectAndChildrens()
        {
            GameObject eventChannelGameObject1 = CreateGameObject();
            GameObject eventChannelGameObject2 = CreateGameObject();
            GameObject childrenGameObject1 = CreateGameObject();
            GameObject childrenGameObject2 = CreateGameObject();
            childrenGameObject1.transform.parent = eventChannelGameObject1.transform;
            childrenGameObject2.transform.parent = eventChannelGameObject2.transform;
            injectionContext.FindGameObjectsWithTag(R.S.Tag.EventChannels).Returns(new List<GameObject>(new[]
            {
                eventChannelGameObject1, eventChannelGameObject2
            }));

            IList<object> actualDependencies = eventChannelScope.GetDependencies(injectionContext, target, typeof(GameObject));

            Assert.AreSame(eventChannelGameObject1, actualDependencies[0]);
            Assert.AreSame(childrenGameObject1, actualDependencies[1]);
            Assert.AreSame(eventChannelGameObject2, actualDependencies[2]);
            Assert.AreSame(childrenGameObject2, actualDependencies[3]);
        }
    }
}