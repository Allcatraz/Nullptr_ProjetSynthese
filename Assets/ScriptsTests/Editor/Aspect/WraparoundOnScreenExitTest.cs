using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class WraparoundOnScreenExitTest : UnitTestCase
    {
        private IRenderer renderer;
        private ITransform transform;
        private ICamera camera;
        private WraparoundOnScreenExit wraparoundOnScreenExit;

        [SetUp]
        public void Before()
        {
            renderer = CreateSubstitute<IRenderer>();
            transform = CreateSubstitute<ITransform>();
            camera = CreateSubstitute<ICamera>();
            wraparoundOnScreenExit = CreateBehaviour<WraparoundOnScreenExit>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            renderer.Received().OnBecameInvisible += Arg.Any<RendererInvisibleEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            renderer.Received().OnBecameInvisible -= Arg.Any<RendererInvisibleEventHandler>();
        }

        [Test]
        public void WhenIsOffScreenAtTopInvertX()
        {
            SetPosition(1, 2, 3);
            SetViewportPosition(2.0f, 0.5f, 0.5f);

            MakeInvisible();

            CheckPosition(-1, 2, 3);
        }

        [Test]
        public void WhenIsOffScreenAtBottomInvertX()
        {
            SetPosition(1, 2, 3);
            SetViewportPosition(-1.0f, 0.5f, 0.5f);

            MakeInvisible();

            CheckPosition(-1, 2, 3);
        }

        [Test]
        public void WhenIsOffScreenAtLeftInvertY()
        {
            SetPosition(1, 2, 3);
            SetViewportPosition(0.5f, -1.0f, 0.5f);

            MakeInvisible();

            CheckPosition(1, -2, 3);
        }

        [Test]
        public void WhenIsOffScreenAtRightInvertY()
        {
            SetPosition(1, 2, 3);
            SetViewportPosition(0.5f, 2.0f, 0.5f);

            MakeInvisible();

            CheckPosition(1, -2, 3);
        }

        private void SetPosition(float x, float y, float z)
        {
            transform.Position.Returns(new Vector3(x, y, z));
        }

        private void SetViewportPosition(float x, float y, float z)
        {
            camera.WorldToViewportPoint(transform.Position).Returns(new Vector3(x, y, z));
        }

        public void Initialize()
        {
            wraparoundOnScreenExit.InjectWraparoundOnScreen(renderer, transform, camera);
            wraparoundOnScreenExit.Awake();
            wraparoundOnScreenExit.OnEnable();
        }

        private void Disable()
        {
            wraparoundOnScreenExit.OnDisable();
        }

        private void MakeInvisible()
        {
            Initialize();
            renderer.OnBecameInvisible += Raise.Event<RendererInvisibleEventHandler>();
        }

        private void CheckPosition(float x, float y, float z)
        {
            transform.Received().Position = Arg.Is<Vector3>(vector => vector.x == x && vector.y == y && vector.z == z);
        }
    }
}