using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class ChangeGravityTest : UnitTestCase
    {
        private static readonly Vector2 Gravity = new Vector2(0, 0);
        private static readonly Vector2 InitialGravity = new Vector2(0, -9.81f);

        private IPhysics2D physics2D;
        private ChangeGravity changeGravity;

        [SetUp]
        public void Before()
        {
            physics2D = CreateSubstitute<IPhysics2D>();
            changeGravity = CreateBehaviour<ChangeGravity>();

            physics2D.Gravity.Returns(InitialGravity);
        }

        [Test]
        public void WhenEnabledSetsTheGravityToZero()
        {
            Initialize();

            CheckGravityIs(Gravity);
        }

        [Test]
        public void WhenDisabledResetsTheGravityToInitialValue()
        {
            Initialize();

            Disable();

            CheckThatGravityReturnedToInitialValue();
        }

        private void Initialize()
        {
            changeGravity.InjectSetGravityAtStart(Gravity, physics2D);
            changeGravity.Awake();
            changeGravity.OnEnable();
        }

        private void Disable()
        {
            changeGravity.OnDisable();
        }

        private void CheckGravityIs(Vector2 gravity)
        {
            physics2D.Received().Gravity = gravity;
        }

        private void CheckThatGravityReturnedToInitialValue()
        {
            physics2D.Received().Gravity = InitialGravity;
        }
    }
}