using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DestroyOnHitStimulusTest : UnitTestCase
    {
        private const int HitPoints = 10;

        private HitStimulus hitStimulus;
        private EntityDestroyer entityDestroyer;
        private DestroyOnHitStimulus destroyOnHitStimulus;

        [SetUp]
        public void Before()
        {
            hitStimulus = CreateSubstitute<HitStimulus>();
            entityDestroyer = CreateSubstitute<EntityDestroyer>();
            destroyOnHitStimulus = CreateBehaviour<DestroyOnHitStimulus>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            hitStimulus.Received().OnHit += Arg.Any<HitStimulusEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            hitStimulus.Received().OnHit -= Arg.Any<HitStimulusEventHandler>();
        }

        [Test]
        public void OnHitDestroyGameObject()
        {
            Initialize();

            Hit();

            CheckGameObjectIsDestroyed();
        }

        private void Initialize()
        {
            destroyOnHitStimulus.InjectDestroyOnHitStimulus(hitStimulus, entityDestroyer);
            destroyOnHitStimulus.Awake();
            destroyOnHitStimulus.OnEnable();
        }

        private void Disable()
        {
            destroyOnHitStimulus.OnDisable();
        }

        public void Hit()
        {
            hitStimulus.OnHit += Raise.Event<HitStimulusEventHandler>(HitPoints);
        }

        private void CheckGameObjectIsDestroyed()
        {
            entityDestroyer.Received().Destroy();
        }
    }
}