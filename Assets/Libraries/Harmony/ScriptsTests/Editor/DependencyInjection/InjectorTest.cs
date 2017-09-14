using System;
using System.Collections.Generic;
using Harmony.Testing;
using NUnit.Framework;
using UnityEngine;

namespace Harmony.Injection
{
    public class InjectorTest : UnitTestCase
    {
        private IInjectionContext injectionContext;
        private Injector injector;

        [SetUp]
        public void Before()
        {
            //Desactivate test mode, because UnitTestCase activates it before each test 
            //and we want to test injection.
            Injector.DisableTestMode();
            ScopeForTests.dependencies.Clear();
            ScopeForTests.gameObjects.Clear();

            injectionContext = CreateSubstitute<IInjectionContext>();
            injector = new Injector(injectionContext);
        }

        [TearDown]
        public void After()
        {
            ScopeForTests.dependencies.Clear();
            ScopeForTests.gameObjects.Clear();
        }

        [Test]
        public void CanInjectDependenciesInTarget()
        {
            TargetBehaviourForTest target = CreateBehaviour<TargetBehaviourForTest>();
            EmptyBehaviour dependency = CreateSubstitute<EmptyBehaviour>();
            ScopeForTests.dependencies.Add(dependency);

            injector.InjectDependencies(target, "Inject");

            Assert.AreSame(dependency, target.receivedDependency);
        }

        [Test]
        public void CanInjectDependenciesInTargetWhenThereIsInheritance()
        {
            InheritedTargetBehaviourForTest target = CreateBehaviour<InheritedTargetBehaviourForTest>();
            EmptyBehaviour dependency = CreateSubstitute<EmptyBehaviour>();
            OtherEmptyBehaviour otherDependency = CreateSubstitute<OtherEmptyBehaviour>();
            ScopeForTests.dependencies.Add(dependency);
            ScopeForTests.dependencies.Add(otherDependency);

            //Should be nothing when no injection is done
            Assert.IsNull(target.receivedDependency);
            Assert.IsNull(target.otherReceivedDependency);

            //Only the base class should have his dependencies resolved
            injector.InjectDependencies(target, "Inject");
            Assert.AreSame(dependency, target.receivedDependency);
            Assert.IsNull(target.otherReceivedDependency);

            //All classes should have their dependencies resolved
            injector.InjectDependencies(target, "OtherInject");
            Assert.AreSame(dependency, target.receivedDependency);
            Assert.AreSame(otherDependency, target.otherReceivedDependency);
        }

        [Test]
        public void ThrowsExceptionIfInjectMethodDoesNotExists()
        {
            TargetBehaviourForTest target = CreateBehaviour<TargetBehaviourForTest>();

            Assert.Throws<ArgumentException>(delegate { injector.InjectDependencies(target, "DoesNotExists"); });
        }

        [Test]
        public void ThrowsExceptionIfDependencyDoesNotExists()
        {
            TargetBehaviourForTest target = CreateBehaviour<TargetBehaviourForTest>();

            Assert.Throws<DependencyNotFoundException>(delegate { injector.InjectDependencies(target, "Inject"); });
        }

        [Test]
        public void ThrowsExceptionIfMoreThanOneDependencyExists()
        {
            TargetBehaviourForTest target = CreateBehaviour<TargetBehaviourForTest>();
            ScopeForTests.dependencies.Add(CreateSubstitute<EmptyBehaviour>());
            ScopeForTests.dependencies.Add(CreateSubstitute<EmptyBehaviour>());

            Assert.Throws<MoreThanOneDependencyFoundException>(delegate { injector.InjectDependencies(target, "Inject"); });
        }

        [Test]
        public void ThrowsExceptionIfParameterCountDoesNotCorrespond()
        {
            ParameterCountBehaviourForTest target = CreateBehaviour<ParameterCountBehaviourForTest>();
            ScopeForTests.dependencies.Add(CreateSubstitute<EmptyBehaviour>());

            Assert.Throws<ArgumentException>(delegate { injector.InjectDependencies(target, "Inject", new object()); });
        }

        [Test]
        public void ThrowsExceptionIfParameterTypesDoesNotCorrespond()
        {
            ParameterTypesBehaviourForTest target = CreateBehaviour<ParameterTypesBehaviourForTest>();
            ScopeForTests.dependencies.Add(CreateSubstitute<EmptyBehaviour>());

            Assert.Throws<ArgumentException>(delegate { injector.InjectDependencies(target, "Inject", new object()); });
        }

        public class TargetBehaviourForTest : UnityScript
        {
            public EmptyBehaviour receivedDependency;

            public void Inject([ScopeForTests] EmptyBehaviour dependency)
            {
                receivedDependency = dependency;
            }
        }

        public class InheritedTargetBehaviourForTest : TargetBehaviourForTest
        {
            public OtherEmptyBehaviour otherReceivedDependency;

            public void OtherInject([ScopeForTests] OtherEmptyBehaviour dependency)
            {
                otherReceivedDependency = dependency;
            }
        }

        public class ParameterCountBehaviourForTest : UnityScript
        {
            public void Inject()
            {
            }
        }

        public class ParameterTypesBehaviourForTest : UnityScript
        {
            public void Inject(string parameter)
            {
            }
        }

        public class ScopeForTests : Scope
        {
            public static List<object> dependencies = new List<object>();
            public static List<GameObject> gameObjects = new List<GameObject>();

            protected override IList<GameObject> GetEligibleGameObjects(IInjectionContext injectionContext, UnityScript target)
            {
                return gameObjects;
            }

            protected override IList<object> GetEligibleDependencies(IInjectionContext injectionContext, UnityScript target, Type dependencyType)
            {
                List<object> eligibleDependencies = new List<object>();
                foreach (object dependency in dependencies)
                {
                    if (dependencyType.IsInstanceOfType(dependency))
                    {
                        eligibleDependencies.Add(dependency);
                    }
                }
                return eligibleDependencies;
            }
        }
    }
}