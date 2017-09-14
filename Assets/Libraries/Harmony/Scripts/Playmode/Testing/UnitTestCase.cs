using System.Collections.Generic;
using Harmony.Injection;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

#pragma warning disable 1587
/// <summary>
/// Module de Harmony visant à simplifier les tests unitaires.
/// </summary>
#pragma warning restore 1587

namespace Harmony.Testing
{
    /// <summary>
    /// Représente un test unitaire.
    /// </summary>
    [NotTested(Reason.TestingTool)]
    public abstract class UnitTestCase
    {
        private readonly IList<GameObject> testGameObjects = new List<GameObject>();

        [SetUp]
        public void BeforeUnitTest()
        {
            Injector.EnableTestMode();
        }

        [TearDown]
        public void AfterUnitTest()
        {
            Injector.DisableTestMode();
            DestroyTestGameObjects();
        }

        [NotNull]
        protected GameObject CreateGameObject()
        {
            GameObject gameObject = new GameObject();
            testGameObjects.Add(gameObject);
            return gameObject;
        }

        [NotNull]
        protected GameObject CreateGameObject([NotNull] string name)
        {
            GameObject gameObject = new GameObject(name);
            testGameObjects.Add(gameObject);
            return gameObject;
        }

        private void DestroyTestGameObjects()
        {
            foreach (GameObject gameObject in testGameObjects)
            {
                Object.DestroyImmediate(gameObject);
            }
            testGameObjects.Clear();
        }

        [NotNull]
        protected T CreateSubstitute<T>() where T : class
        {
            return Substitute.For<T>();
        }

        [NotNull]
        protected T CreateBehaviour<T>() where T : UnityScript
        {
            return CreateGameObject().AddComponent<T>();
        }

        public class EmptyBehaviour : UnityScript
        {
        }

        public class OtherEmptyBehaviour : UnityScript
        {
        }
    }
}