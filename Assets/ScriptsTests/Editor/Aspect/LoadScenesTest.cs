using Harmony;
using Harmony.Testing;
using NUnit.Framework;
using NSubstitute;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    public class LoadScenesTest : UnitTestCase
    {
        private IHierachy hierachy;
        private LoadScenes loadScenes;

        [SetUp]
        public void Before()
        {
            hierachy = CreateSubstitute<IHierachy>();
            loadScenes = CreateBehaviour<LoadScenes>();
        }

        [Test]
        public void LoadSceneAtStart()
        {
            LoadScene(new[] { R.E.Scene.Main }, LoadSceneMode.Additive);

            HasLoadedScene(R.S.Scene.Main, LoadSceneMode.Additive);
        }

        [Test]
        public void CanLoadSceneInSingleMode()
        {
            LoadScene(new[] { R.E.Scene.Main }, LoadSceneMode.Single);

            HasLoadedScene(R.S.Scene.Main, LoadSceneMode.Single);
        }

        [Test]
        public void CanLoadSceneInAdditiveMode()
        {
            LoadScene(new[] { R.E.Scene.Main }, LoadSceneMode.Additive);

            HasLoadedScene(R.S.Scene.Main, LoadSceneMode.Additive);
        }

        [Test]
        public void CanLoadMultipleScenes()
        {
            LoadScene(new[] { R.E.Scene.Main, R.E.Scene.LoadingScreen }, LoadSceneMode.Additive);

            HasLoadedScene(R.S.Scene.Main, LoadSceneMode.Additive);
            HasLoadedScene(R.S.Scene.LoadingScreen, LoadSceneMode.Additive);
        }

        public void LoadScene(R.E.Scene[] scene, LoadSceneMode mode)
        {
            loadScenes.InjectLoadSceneAtStart(scene, mode, hierachy);
            loadScenes.Awake();
            loadScenes.Start();
        }

        public void HasLoadedScene(string sceneName, LoadSceneMode mode)
        {
            hierachy.Received().LoadScene(sceneName, mode);
        }

        public void HasNotLoadedScene(string sceneName, LoadSceneMode mode)
        {
            hierachy.Received(0).LoadScene(sceneName, mode);
        }
    }
}