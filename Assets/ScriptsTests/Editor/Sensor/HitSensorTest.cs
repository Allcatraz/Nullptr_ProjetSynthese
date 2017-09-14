using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class HitSensorTest : UnitTestCase
    {
        private const int HitPoints = 10;

        private ICollider2D collider2D;
        private HitSensor hitSensor;

        [SetUp]
        public void Before()
        {
            collider2D = CreateSubstitute<ICollider2D>();
            hitSensor = CreateBehaviour<HitSensor>();
        }

        [Test]
        public void WhenCreatedSetGameObjectLayerToHitboxLayer()
        {
            Initialize();

            CheckIsInHitSensorLayer();
        }

        [Test]
        public void WhenCreatedSetsColliderToTrigger()
        {
            Initialize();

            CheckIsATrigger();
        }

        [Test]
        public void WhenHitTriggerHitEvent()
        {
            HitSensorEventHandler hitSensorEventHandler = CreateSubstitute<HitSensorEventHandler>();
            Initialize();
            hitSensor.OnHit += hitSensorEventHandler;

            hitSensor.Hit(HitPoints);

            hitSensorEventHandler.Received()(HitPoints);
        }

        private void Initialize()
        {
            hitSensor.InjectHitSensor(collider2D);
            hitSensor.Awake();
        }

        private void CheckIsInHitSensorLayer()
        {
            Assert.AreEqual(LayerMask.NameToLayer(R.S.Layer.HitSensor), hitSensor.gameObject.layer);
        }

        private void CheckIsATrigger()
        {
            collider2D.Received().IsTrigger = true;
        }
    }
}