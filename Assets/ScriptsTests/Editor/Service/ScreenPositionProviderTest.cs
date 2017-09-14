using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class ScreenPositionProviderTest : UnitTestCase
    {
        private const float ObjectSize = 2;
        private static readonly Vector2 RandomPositionInViewport = new Vector3(1, 2);
        private static readonly Vector3 RandomPositionInCamera = new Vector3(4, 5, 6);
        private static readonly Vector3 ViewportOrigin = new Vector2(-20, -20);
        private static readonly Vector3 ViewportSize = new Vector2(5, 10);
        private static readonly Vector2 ViewportPosition = new Vector2(5, 10);
        private static readonly Vector3 CameraPosition = new Vector3(15, 20, 25);

        private ICamera camera;
        private IRandom random;
        private ScreenPositionProvider screenPositionProvider;

        [SetUp]
        public void Before()
        {
            camera = CreateSubstitute<ICamera>();
            random = CreateSubstitute<IRandom>();
            screenPositionProvider = CreateBehaviour<ScreenPositionProvider>();

            random.GetRandomPosition(0, 1, 0, 1).Returns(RandomPositionInViewport);
            camera.ViewportToWorldPoint(RandomPositionInViewport).Returns(RandomPositionInCamera);

            camera.ScreenToWorldPoint(Arg.Any<Vector3>()).Returns(ViewportOrigin);
            camera.WorldToViewportPoint(Arg.Any<Vector3>()).Returns(ViewportSize);
            random.GetRandomPositionOnRectangleEdge(Arg.Any<Vector2>(),
                                                    Arg.Any<float>(),
                                                    Arg.Any<float>()).Returns(ViewportPosition);
            camera.ViewportToWorldPoint(ViewportPosition).Returns(CameraPosition);
        }

        [Test]
        public void CanReturnRandomPositionInScreen()
        {
            Initialize();

            Vector2 position = screenPositionProvider.GetRandomInScreenPosition();

            Assert.AreEqual(RandomPositionInCamera.x, position.x);
            Assert.AreEqual(RandomPositionInCamera.y, position.y);
        }

        [Test]
        public void CanReturnPositionOffScreen()
        {
            Initialize();

            Vector2 position = screenPositionProvider.GetRandomOffScreenPosition(ObjectSize);

            Assert.AreEqual(CameraPosition.x, position.x);
            Assert.AreEqual(CameraPosition.y, position.y);
        }

        private void Initialize()
        {
            screenPositionProvider.InjectOutOfScreenSpawner(camera,
                                                            random);
            screenPositionProvider.Awake();
        }
    }
}