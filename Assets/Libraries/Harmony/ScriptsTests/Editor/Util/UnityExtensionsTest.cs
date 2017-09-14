using System.Collections.Generic;
using Harmony.Testing;
using NUnit.Framework;
using UnityEngine;

namespace Harmony.Util
{
    public class UnityExtensionsTest : UnitTestCase
    {
        [Test]
        public void CanGetComponentInTopParent()
        {
            GameObject topParent = CreateGameObject();
            GameObject children = CreateGameObject();
            GameObject bottomChildren = CreateGameObject();
            children.transform.parent = topParent.transform;
            bottomChildren.transform.parent = children.transform;

            var componentInTopParent = topParent.AddComponent<EmptyBehaviour1>();
            topParent.AddComponent<EmptyBehaviour2>();

            Assert.AreSame(componentInTopParent, bottomChildren.GetComponentInTopParent<EmptyBehaviour1>());

            Assert.AreEqual(componentInTopParent, bottomChildren.GetComponentInTopParent(typeof(EmptyBehaviour1)));
        }

        [Test]
        public void CanGetComponentsInTopParent()
        {
            GameObject topParent = CreateGameObject();
            GameObject children = CreateGameObject();
            GameObject bottomChildren = CreateGameObject();
            children.transform.parent = topParent.transform;
            bottomChildren.transform.parent = children.transform;

            UnityScript[] componentsInTopParent =
            {
                topParent.AddComponent<EmptyBehaviour1>(),
                topParent.AddComponent<EmptyBehaviour1>()
            };
            topParent.AddComponent<EmptyBehaviour2>();
            topParent.AddComponent<EmptyBehaviour2>();

            Assert.Contains(componentsInTopParent[0], bottomChildren.GetComponentsInTopParent<EmptyBehaviour1>());
            Assert.Contains(componentsInTopParent[1], bottomChildren.GetComponentsInTopParent<EmptyBehaviour1>());

            Assert.Contains(componentsInTopParent[0], bottomChildren.GetComponentsInTopParent(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInTopParent[1], bottomChildren.GetComponentsInTopParent(typeof(EmptyBehaviour1)));
        }

        [Test]
        public void CanGetComponentInParentChildrenOrSibbling()
        {
            GameObject topParent = CreateGameObject();
            GameObject children1 = CreateGameObject();
            GameObject children2 = CreateGameObject();
            GameObject bottomChildren = CreateGameObject();
            children1.transform.parent = topParent.transform;
            children2.transform.parent = topParent.transform;
            bottomChildren.transform.parent = children1.transform;

            var componentInParent = topParent.AddComponent<EmptyBehaviour1>();
            var componentInChildren1 = children1.AddComponent<EmptyBehaviour2>();
            var componentInChildren2 = children2.AddComponent<EmptyBehaviour3>();
            var componentInBottomChildren = bottomChildren.AddComponent<EmptyBehaviour4>();

            Assert.AreEqual(componentInParent, bottomChildren.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.AreEqual(componentInChildren1, bottomChildren.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.AreEqual(componentInChildren2, bottomChildren.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.AreEqual(componentInBottomChildren, bottomChildren.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.AreEqual(componentInParent, bottomChildren.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.AreEqual(componentInChildren1, bottomChildren.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.AreEqual(componentInChildren2, bottomChildren.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.AreEqual(componentInBottomChildren, bottomChildren.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));

            Assert.AreEqual(componentInParent, children1.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.AreEqual(componentInChildren1, children1.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.AreEqual(componentInChildren2, children1.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.AreEqual(componentInBottomChildren, children1.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.AreEqual(componentInParent, children1.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.AreEqual(componentInChildren1, children1.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.AreEqual(componentInChildren2, children1.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.AreEqual(componentInBottomChildren, children1.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));

            Assert.AreEqual(componentInParent, children2.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.AreEqual(componentInChildren1, children2.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.AreEqual(componentInChildren2, children2.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.AreEqual(componentInBottomChildren, children2.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.AreEqual(componentInParent, children2.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.AreEqual(componentInChildren1, children2.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.AreEqual(componentInChildren2, children2.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.AreEqual(componentInBottomChildren, children2.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));

            Assert.AreEqual(componentInParent, topParent.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.AreEqual(componentInChildren1, topParent.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.AreEqual(componentInChildren2, topParent.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.AreEqual(componentInBottomChildren, topParent.GetComponentInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.AreEqual(componentInParent, topParent.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.AreEqual(componentInChildren1, topParent.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.AreEqual(componentInChildren2, topParent.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.AreEqual(componentInBottomChildren, topParent.GetComponentInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));
        }

        [Test]
        public void CanGetComponentsInParentChildrenOrSibbling()
        {
            GameObject topParent = CreateGameObject();
            GameObject children1 = CreateGameObject();
            GameObject children2 = CreateGameObject();
            GameObject bottomChildren = CreateGameObject();
            children1.transform.parent = topParent.transform;
            children2.transform.parent = topParent.transform;
            bottomChildren.transform.parent = children1.transform;

            EmptyBehaviour1[] componentsInParent =
            {
                topParent.AddComponent<EmptyBehaviour1>(),
                topParent.AddComponent<EmptyBehaviour1>()
            };
            EmptyBehaviour2[] componentsInChildren1 =
            {
                children1.AddComponent<EmptyBehaviour2>(),
                children1.AddComponent<EmptyBehaviour2>()
            };
            EmptyBehaviour3[] componentsInChildren2 =
            {
                children2.AddComponent<EmptyBehaviour3>(),
                children2.AddComponent<EmptyBehaviour3>()
            };
            EmptyBehaviour4[] componentsInBottomChildren =
            {
                bottomChildren.AddComponent<EmptyBehaviour4>(),
                bottomChildren.AddComponent<EmptyBehaviour4>()
            };

            Assert.Contains(componentsInParent[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInParent[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInChildren1[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren1[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren2[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInChildren2[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInBottomChildren[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());
            Assert.Contains(componentsInBottomChildren[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.Contains(componentsInParent[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInParent[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInChildren1[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren1[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren2[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInChildren2[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInBottomChildren[0], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));
            Assert.Contains(componentsInBottomChildren[1], bottomChildren.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));

            Assert.Contains(componentsInParent[0], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInParent[1], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInChildren1[0], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren1[1], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren2[0], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInChildren2[1], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInBottomChildren[0], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());
            Assert.Contains(componentsInBottomChildren[1], children1.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.Contains(componentsInParent[0], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInParent[1], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInChildren1[0], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren1[1], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren2[0], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInChildren2[1], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInBottomChildren[0], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));
            Assert.Contains(componentsInBottomChildren[1], children1.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));

            Assert.Contains(componentsInParent[0], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInParent[1], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInChildren1[0], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren1[1], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren2[0], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInChildren2[1], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInBottomChildren[0], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());
            Assert.Contains(componentsInBottomChildren[1], children2.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.Contains(componentsInParent[0], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInParent[1], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInChildren1[0], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren1[1], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren2[0], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInChildren2[1], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInBottomChildren[0], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));
            Assert.Contains(componentsInBottomChildren[1], children2.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));

            Assert.Contains(componentsInParent[0], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInParent[1], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour1>());
            Assert.Contains(componentsInChildren1[0], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren1[1], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour2>());
            Assert.Contains(componentsInChildren2[0], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInChildren2[1], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour3>());
            Assert.Contains(componentsInBottomChildren[0], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());
            Assert.Contains(componentsInBottomChildren[1], topParent.GetComponentsInChildrensParentsOrSiblings<EmptyBehaviour4>());

            Assert.Contains(componentsInParent[0], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInParent[1], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour1)));
            Assert.Contains(componentsInChildren1[0], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren1[1], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour2)));
            Assert.Contains(componentsInChildren2[0], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInChildren2[1], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour3)));
            Assert.Contains(componentsInBottomChildren[0], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));
            Assert.Contains(componentsInBottomChildren[1], topParent.GetComponentsInChildrensParentsOrSiblings(typeof(EmptyBehaviour4)));
        }

        [Test]
        public void CanGetTopParent()
        {
            GameObject topParent = CreateGameObject();
            GameObject children = CreateGameObject();
            GameObject bottomChildren = CreateGameObject();
            children.transform.parent = topParent.transform;
            bottomChildren.transform.parent = children.transform;

            Assert.AreSame(topParent, bottomChildren.GetTopParent());
        }

        [Test]
        public void CanGetAllChildren()
        {
            GameObject topParent = CreateGameObject();
            GameObject children = CreateGameObject();
            GameObject bottomChildren = CreateGameObject();
            children.transform.parent = topParent.transform;
            bottomChildren.transform.parent = children.transform;

            IList<GameObject> childrens = topParent.GetAllChildrens();

            Assert.AreSame(children, childrens[0]);
            Assert.AreSame(bottomChildren, childrens[1]);
        }

        [Test]
        public void CanGetAllHierachy()
        {
            GameObject topParent = CreateGameObject();
            GameObject children = CreateGameObject();
            GameObject bottomChildren = CreateGameObject();
            children.transform.parent = topParent.transform;
            bottomChildren.transform.parent = children.transform;

            IList<GameObject> childrens = topParent.GetAllHierachy();

            Assert.AreSame(topParent, childrens[0]);
            Assert.AreSame(children, childrens[1]);
            Assert.AreSame(bottomChildren, childrens[2]);
        }

        private class EmptyBehaviour1 : UnityScript
        {
        }

        private class EmptyBehaviour2 : UnityScript
        {
        }

        private class EmptyBehaviour3 : UnityScript
        {
        }

        private class EmptyBehaviour4 : UnityScript
        {
        }
    }
}