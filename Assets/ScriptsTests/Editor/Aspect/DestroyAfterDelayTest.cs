using System.Collections;
using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DestroyAfterDelayTest : UnitTestCase
    {
        private const int DelayBeforeDeathInSeconds = 1;

        private EntityDestroyer entityDestroyer;
        private ICoroutineExecutor coroutineExecutor;
        private DestroyAfterDelay destroyAfterDelay;

        private IEnumerator coroutine;

        [SetUp]
        public void Before()
        {
            entityDestroyer = CreateSubstitute<EntityDestroyer>();
            coroutineExecutor = CreateSubstitute<ICoroutineExecutor>();
            destroyAfterDelay = CreateBehaviour<DestroyAfterDelay>();

            //Capture the Coroutine, so we can execute it manualy during the test.
            coroutineExecutor.StartCoroutine(Arg.Is(destroyAfterDelay),
                                             Arg.Do<IEnumerator>(arg => coroutine = arg));
        }

        [Test]
        public void GameObjectMustBeDestroyedAfterDelay()
        {
            Initialize();

            MakeTimePassUntilDestruction();

            CheckWasDestroyed();
        }

        private void Initialize()
        {
            destroyAfterDelay.InjectDestroyAfterDelay(DelayBeforeDeathInSeconds,
                                                      entityDestroyer,
                                                      coroutineExecutor);

            destroyAfterDelay.Awake();
            destroyAfterDelay.Start();
        }

        private void MakeTimePassUntilDestruction()
        {
            while (coroutine.MoveNext())
            {
            }
        }

        private void CheckWasDestroyed()
        {
            entityDestroyer.Received().Destroy();
        }
    }
}