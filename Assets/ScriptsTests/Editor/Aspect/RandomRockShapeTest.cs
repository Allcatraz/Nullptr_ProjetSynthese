using System.Collections.Generic;
using Harmony;
using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class RandomRockShapeTest : UnitTestCase
    {
        private const float Radius = 5.2f;
        private const int Roughness = 50;
        private const int MinAngle = 5;
        private const int MaxAngle = 15;

        private ILineRenderer lineRenderer;
        private IPolygonCollider2D collider2D;
        private IPolygonCollider2D trigger2D;
        private IRandom random;
        private RandomRockShape randomRockShape;

        [SetUp]
        public void Before()
        {
            lineRenderer = CreateSubstitute<ILineRenderer>();
            collider2D = CreateSubstitute<IPolygonCollider2D>();
            trigger2D = CreateSubstitute<IPolygonCollider2D>();
            random = new UnityRandom(); //We do not use a Mock here, because random generation is not tested.
            randomRockShape = CreateBehaviour<RandomRockShape>();
        }

        [Test]
        public void AtStartCreateRandomShape()
        {
            Initialize();

            CheckLineRendererHasReceivedPoints();
            CheckColliderHasReceivedPoints();
            CheckTriggerHasReceivedPoints();
        }

        private void Initialize()
        {
            randomRockShape.InjectRandomRockShape(Radius,
                                                  Roughness,
                                                  MinAngle,
                                                  MaxAngle,
                                                  lineRenderer,
                                                  collider2D,
                                                  trigger2D,
                                                  random);
            randomRockShape.Awake();
            randomRockShape.Start();
        }

        private void CheckLineRendererHasReceivedPoints()
        {
            lineRenderer.Received().SetPoints(Arg.Any<IList<Vector3>>());
        }

        private void CheckColliderHasReceivedPoints()
        {
            collider2D.Received().SetPoints(Arg.Any<IList<Vector2>>());
        }

        private void CheckTriggerHasReceivedPoints()
        {
            trigger2D.Received().SetPoints(Arg.Any<IList<Vector2>>());
        }
    }
}