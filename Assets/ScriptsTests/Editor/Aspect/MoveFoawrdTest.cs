using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class MoveFoawrdTest : UnitTestCase
    {
        private TranslateMover translateMover;
        private MoveFoward moveFoward;

        [SetUp]
        public void Before()
        {
            translateMover = CreateSubstitute<TranslateMover>();
            moveFoward = CreateBehaviour<MoveFoward>();
        }

        [Test]
        public void BulletAllwaysMoveFoward()
        {
            Initialize();

            moveFoward.Update();

            CheckBulletMovedFoward();
        }

        private void Initialize()
        {
            moveFoward.InjectBulletController(translateMover);

            moveFoward.Awake();
        }

        private void CheckBulletMovedFoward()
        {
            translateMover.Received().MoveFoward();
        }
    }
}