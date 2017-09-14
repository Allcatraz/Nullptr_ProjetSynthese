using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DestroyOnHitTest : UnitTestCase
    {
        private const int HitPoints = 10;

        private HitSensor hitSensor;
        private EntityDestroyer entityDestroyer;
        private DestroyOnHit destroyOnHit;

        [SetUp]
        public void Before()
        {
            hitSensor = CreateSubstitute<HitSensor>();
            entityDestroyer = CreateSubstitute<EntityDestroyer>();
            destroyOnHit = CreateBehaviour<DestroyOnHit>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            hitSensor.Received().OnHit += Arg.Any<HitSensorEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            hitSensor.Received().OnHit -= Arg.Any<HitSensorEventHandler>();
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
            destroyOnHit.InjectDestroyOnHit(hitSensor, entityDestroyer);
            destroyOnHit.Awake();
            destroyOnHit.OnEnable();
        }

        private void Disable()
        {
            destroyOnHit.OnDisable();
        }

        public void Hit()
        {
            hitSensor.OnHit += Raise.Event<HitSensorEventHandler>(HitPoints);
        }

        private void CheckGameObjectIsDestroyed()
        {
            entityDestroyer.Received().Destroy();
        }
    }
}