using System.Collections;
using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DestroyAfterTimeOutOfScreenTest : UnitTestCase
    {
        private const int DelayBeforeDeathInSeconds = 1;

        private IRenderer renderer;
        private ICoroutineExecutor coroutineExecutor;
        private EntityDestroyer entityDestroyer;
        private ICoroutine coroutine;
        private DestroyAfterTimeOutOfScreen destroyAfterTimeOutOfScreen;

        private IEnumerator coroutineEnumetator;

        [SetUp]
        public void Before()
        {
            renderer = CreateSubstitute<IRenderer>();
            coroutineExecutor = CreateSubstitute<ICoroutineExecutor>();
            entityDestroyer = CreateSubstitute<EntityDestroyer>();
            coroutine = CreateSubstitute<ICoroutine>();
            destroyAfterTimeOutOfScreen = new DestroyAfterTimeOutOfScreen();

            //Capture the Coroutine, so we can execute it manualy during the test.
            coroutineExecutor.StartCoroutine(Arg.Is(destroyAfterTimeOutOfScreen),
                                             Arg.Do<IEnumerator>(arg => coroutineEnumetator = arg)).Returns(coroutine);
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            renderer.Received().OnBecameVisible += Arg.Any<RendererVisibleEventHandler>();
            renderer.Received().OnBecameInvisible += Arg.Any<RendererInvisibleEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            renderer.Received().OnBecameVisible -= Arg.Any<RendererVisibleEventHandler>();
            renderer.Received().OnBecameInvisible -= Arg.Any<RendererInvisibleEventHandler>();
        }

        [Test]
        public void DestroyTopParentAfterTimeOutOfScreen()
        {
            Initialize();

            MakeOutOfScreen();
            MakeTimePassUntilDestruction();

            CheckWasDestroyed();
        }

        [Test]
        public void StopsDestructionWhenGoInScreen()
        {
            Initialize();

            MakeOutOfScreen();
            MakeInScreen();

            CheckWasNotDestroyed();
            CheckDestructionCancelled();
        }

        private void Initialize()
        {
            destroyAfterTimeOutOfScreen.InjectDestroyAfterTimeOutOfScreen(DelayBeforeDeathInSeconds,
                                                                          renderer,
                                                                          entityDestroyer,
                                                                          coroutineExecutor);
            destroyAfterTimeOutOfScreen.Awake();
            destroyAfterTimeOutOfScreen.OnEnable();
        }

        private void Disable()
        {
            destroyAfterTimeOutOfScreen.OnDisable();
        }

        private void MakeOutOfScreen()
        {
            renderer.OnBecameInvisible += Raise.Event<RendererInvisibleEventHandler>();
        }

        private void MakeInScreen()
        {
            renderer.OnBecameVisible += Raise.Event<RendererVisibleEventHandler>();
        }

        private void MakeTimePassUntilDestruction()
        {
            while (coroutineEnumetator.MoveNext())
            {
            }
        }

        private void CheckDestructionCancelled()
        {
            coroutine.Received().Stop();
        }

        private void CheckWasDestroyed()
        {
            entityDestroyer.Received().Destroy();
        }

        private void CheckWasNotDestroyed()
        {
            entityDestroyer.Received(0).Destroy();
        }
    }
}