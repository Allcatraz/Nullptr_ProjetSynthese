using Harmony;
using Harmony.EventSystem;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class AddScoreOnPrefabDeathTest : UnitTestCase
    {
        private const R.E.Prefab GoodPrefab = R.E.Prefab.Player;
        private const R.E.Prefab WrongPrefab = R.E.Prefab.Rock;
        private const uint PointsPerPrefab = 100;

        private Score score;
        private DeathEventChannel deathEventChannel;
        private AddScoreOnPrefabDeath addScoreOnPrefabDeath;

        [SetUp]
        public void Before()
        {
            score = CreateSubstitute<Score>();
            deathEventChannel = CreateSubstitute<DeathEventChannel>();
            addScoreOnPrefabDeath = CreateBehaviour<AddScoreOnPrefabDeath>();
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            deathEventChannel.Received().OnEventPublished += Arg.Any<EventChannelHandler<DeathEvent>>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            deathEventChannel.Received().OnEventPublished -= Arg.Any<EventChannelHandler<DeathEvent>>();
        }

        [Test]
        public void WhenPrefabDiesAddPointsToScore()
        {
            Initialize();

            MakeGoodPrefabDie();

            CheckPointsAdded(PointsPerPrefab);
        }

        [Test]
        public void WhenWrongPrefabDiesAddPointsToScore()
        {
            Initialize();

            MakeWrongPrefabDie();

            CheckPointsNotAdded(PointsPerPrefab);
        }

        private void Initialize()
        {
            
            addScoreOnPrefabDeath.InjectAddScoreOnPrefabDeath(GoodPrefab,
                                                              PointsPerPrefab,
                                                              score,
                                                              deathEventChannel);
                                                              
            addScoreOnPrefabDeath.Awake();
            addScoreOnPrefabDeath.OnEnable();
        }

        private void Disable()
        {
            addScoreOnPrefabDeath.OnDisable();
        }

        private void MakeGoodPrefabDie()
        {
            deathEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<DeathEvent>>(new DeathEvent(GoodPrefab));
        }

        private void MakeWrongPrefabDie()
        {
            deathEventChannel.OnEventPublished += Raise.Event<EventChannelHandler<DeathEvent>>(new DeathEvent(WrongPrefab));
        }

        private void CheckPointsAdded(uint points)
        {
            score.Received().AddPoints(points);
        }

        private void CheckPointsNotAdded(uint points)
        {
            score.Received(0).AddPoints(points);
        }
    }
}