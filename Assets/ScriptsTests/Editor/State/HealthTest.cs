using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class HealthTest : UnitTestCase
    {
        private Health health;

        [SetUp]
        public void Before()
        {
            health = CreateBehaviour<Health>();
            health.InjectHealth(100,100);
            health.Awake();
        }

        [Test]
        public void DiminishHealthWhenHit()
        {
            health.Heal(100);

            health.Hit(10);

            Assert.AreEqual(90, health.HealthPoints);
        }

        [Test]
        public void CantGoHigherThanMaxHealth()
        {
            health.Heal(10);
            Assert.AreEqual(100, health.HealthPoints);
        }

        [Test]
        public void CantGoLowerThanZero()
        {
            health.Hit(1000);
            Assert.AreEqual(0, health.HealthPoints);
        }

        [Test]
        public void WhenHealthChangesEventIsTriggered()
        {
            health.Heal(100);
            HealthChangedEventHandler healthChangedEventHandler = CreateSubstitute<HealthChangedEventHandler>();
            health.OnHealthChanged += healthChangedEventHandler;

            health.Hit(10);

            healthChangedEventHandler.Received()(100, 90);
        }

        [Test]
        public void WhenHealthPointsReachesZeroDeathEventIsTriggered()
        {
            DeathEventHandler deathEventHandler = CreateSubstitute<DeathEventHandler>();
            health.OnDeath += deathEventHandler;

            health.Heal(100);
            health.Hit(1000);

            deathEventHandler.Received()();
        }
    }
}