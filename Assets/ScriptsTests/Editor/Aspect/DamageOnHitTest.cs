using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class DamageOnHitTest : UnitTestCase
    {
        private const int HitPoints = 10;

        private Health health;
        private HitSensor hitSensor;
        private DamageOnHit damageOnHit;

        [SetUp]
        public void Before()
        {
            health = CreateSubstitute<Health>();
            hitSensor = CreateSubstitute<HitSensor>();
            damageOnHit = CreateBehaviour<DamageOnHit>();
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
        public void WhenReceiveHitSendHitPointsToHealth()
        {
            Initialize();

            ReceiveHit(HitPoints);

            CheckHealthReceivedHit(HitPoints);
        }

        private void Initialize()
        {
            damageOnHit.InjectDamageOnHit(health,hitSensor);
            damageOnHit.Awake();
            damageOnHit.OnEnable();
        }

        private void Disable()
        {
            damageOnHit.OnDisable();
        }

        private void ReceiveHit(int hitPoints)
        {
            hitSensor.OnHit += Raise.Event<HitSensorEventHandler>(hitPoints);
        }

        private void CheckHealthReceivedHit(int hitPoints)
        {
            health.Received().Hit(hitPoints);
        }
    }
}