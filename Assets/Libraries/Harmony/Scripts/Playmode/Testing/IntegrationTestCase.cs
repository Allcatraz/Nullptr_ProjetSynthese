using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony.Testing
{
    /// <summary>
    /// Représente un test d'intégration.
    /// </summary>
    /// <typeparam name="Configuration">Component à utiliser pour configurer la scène.</typeparam>
    [NotTested(Reason.TestingTool)]
    public abstract class IntegrationTestCase<Configuration> where Configuration : UnityScript
    {
        private Scene scene;

        [SetUp]
        public void BeforeIntegrationTestCase()
        {
            scene = SceneManager.CreateScene("IntegrationTests");
            SceneManager.SetActiveScene(scene);

            CreateGameObject().AddComponent<Configuration>();
        }

        [TearDown]
        public void AfterIntegrationTestCase()
        {
#pragma warning disable 618
            SceneManager.UnloadScene(scene);
#pragma warning restore 618
        }

        [NotNull]
        protected GameObject CreateGameObject()
        {
            return new GameObject();
        }

        [NotNull]
        protected GameObject CreateGameObject([NotNull] string name)
        {
            return new GameObject(name);
        }

        [NotNull]
        protected T CreateBehaviour<T>() where T : UnityScript
        {
            return CreateGameObject().AddComponent<T>();
        }

        protected class EmptyBehaviour : UnityScript
        {
        }

        protected class OtherEmptyBehaviour : UnityScript
        {
        }
    }
}