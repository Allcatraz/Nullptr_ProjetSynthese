using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class TranslateMoverTest : UnitTestCase
    {
        private const int Speed = 10;
        private static readonly Vector3 FowardVector = new Vector3(15, 20, 25);
        private const float DeltaTime = 30;

        private ITransform transform;
        private IRigidbody2D rigidbody2D;
        private ITime time;
        private TranslateMover translateMover;

        [SetUp]
        public void Before()
        {
            transform = CreateSubstitute<ITransform>();
            rigidbody2D = CreateSubstitute<IRigidbody2D>();
            time = CreateSubstitute<ITime>();
            translateMover = CreateBehaviour<TranslateMover>();

            transform.Up.Returns(FowardVector);
            time.DeltaTime.Returns(DeltaTime);
        }

        [Test]
        public void WhenMoveFowardTranslateFowardAccordingToSpeed()
        {
            Initilize();

            translateMover.MoveFoward();

            CheckHasMovedOf(Speed * FowardVector.x * DeltaTime,
                            Speed * FowardVector.y * DeltaTime);
        }

        private void Initilize()
        {
            translateMover.InjectTranslateMover(Speed, transform, rigidbody2D, time);
            translateMover.Awake();
        }

        private void CheckHasMovedOf(float x, float y)
        {
            rigidbody2D.Received().Translate(Arg.Is<Vector2>(vector => vector.x == x && vector.y == y));
        }
    }
}