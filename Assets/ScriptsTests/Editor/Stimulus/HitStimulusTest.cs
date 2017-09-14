using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class HitStimulusTest : UnitTestCase
    {
        private const int HitPoints = 10;

        private ICollider2D collider2D;
        private HitStimulus hitStimulus;

        [SetUp]
        public void Before()
        {
            collider2D = CreateSubstitute<ICollider2D>();
            hitStimulus = CreateBehaviour<HitStimulus>();
        }

        [Test]
        public void WhenStartingRegistersToEvents()
        {
            Initialize();

            collider2D.Received().OnTriggerEntered += Arg.Any<Collider2DTriggerEventHandler>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            collider2D.Received().OnTriggerEntered -= Arg.Any<Collider2DTriggerEventHandler>();
        }

        [Test]
        public void SetGameObjectLayerToHitboxLayerAtStart()
        {
            Initialize();

            CheckIsInHitSensorLayer();
        }

        [Test]
        public void SetsColliderToTriggerAtStart()
        {
            Initialize();

            CheckIsATrigger();
        }

        [Test]
        public void WhenHitSendHitToSensor()
        {
            ICollider2D hitSensorCollider2D = CreateSubstitute<ICollider2D>();
            HitSensor hitSensor = CreateSubstitute<HitSensor>();
            hitSensorCollider2D.GetOtherComponent<HitSensor>().Returns(hitSensor);
            HitStimulusEventHandler hitEventEventHandler = CreateSubstitute<HitStimulusEventHandler>();
            Initialize();
            hitStimulus.OnHit += hitEventEventHandler;

            collider2D.OnTriggerEntered += Raise.Event<Collider2DTriggerEventHandler>(hitSensorCollider2D);

            hitEventEventHandler.Received()(HitPoints);
        }

        [Test]
        public void WhenHitTriggerHitEvent()
        {
            ICollider2D hitSensorCollider2D = CreateSubstitute<ICollider2D>();
            HitSensor hitSensor = CreateSubstitute<HitSensor>();
            hitSensorCollider2D.GetOtherComponent<HitSensor>().Returns(hitSensor);
            Initialize();

            collider2D.OnTriggerEntered += Raise.Event<Collider2DTriggerEventHandler>(hitSensorCollider2D);

            hitSensor.Received().Hit(HitPoints);
        }

        private void Initialize()
        {
            hitStimulus.InjectHitStimulus(HitPoints, collider2D);
            hitStimulus.Awake();
        }

        private void Destroy()
        {
            hitStimulus.OnDestroy();
        }

        private void CheckIsInHitSensorLayer()
        {
            Assert.AreEqual(LayerMask.NameToLayer(R.S.Layer.HitSensor), hitStimulus.gameObject.layer);
        }

        private void CheckIsATrigger()
        {
            collider2D.Received().IsTrigger = true;
        }
    }
}