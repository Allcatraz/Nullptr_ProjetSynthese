using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class HealthBarViewTest : UnitTestCase
    {
        private ITransform barTransform;
        private HealthBarView healthBarView;

        [SetUp]
        public void Before()
        {
            barTransform = CreateSubstitute<ITransform>();
            healthBarView = CreateBehaviour<HealthBarView>();
        }

        [Test]
        public void BarLengthIsEqualToPercentage()
        {
            Initialize();
            SetScaleTo(1, 2, 3);

            healthBarView.SetHealthPercentage(0.5f);

            CheckScaleIs(0.5f, 2, 3);
        }

        private void Initialize()
        {
            healthBarView.InjectHealthBar(barTransform);
        }

        private void SetScaleTo(float x, float y, float z)
        {
            barTransform.LocalScale.Returns(new Vector3(x, y, z));
        }

        private void CheckScaleIs(float x, float y, float z)
        {
            barTransform.Received().LocalScale = Arg.Is<Vector3>(vector3 => vector3.x == x && vector3.y == y && vector3.z == z);
        }
    }
}