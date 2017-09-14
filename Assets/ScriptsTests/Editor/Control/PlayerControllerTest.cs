using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class PlayerControllerTest : UnitTestCase
    {
        private Health health;
        private PlayerInputSensor playerInputSensor;
        private PhysicsMover physicsMover;
        private ProjectileShooter projectileShooter;
        private IInputDevice playersInputDevice;
        private PlayerController playerController;

        [SetUp]
        public void Before()
        {
            health = CreateSubstitute<Health>();
            playerInputSensor = CreateSubstitute<PlayerInputSensor>();
            physicsMover = CreateSubstitute<PhysicsMover>();
            projectileShooter = CreateSubstitute<ProjectileShooter>();
            playersInputDevice = CreateSubstitute<IInputDevice>();
            playerController = CreateBehaviour<PlayerController>();

            playerInputSensor.Players.Returns(playersInputDevice);
        }

        [Test]
        public void WhenCreatedRegistersToEvents()
        {
            Initialize();

            playersInputDevice.Received().OnFoward += Arg.Any<FowardEventHandler>();
            playersInputDevice.Received().OnBackward += Arg.Any<BackwardEventHandler>();
            playersInputDevice.Received().OnRotateLeft += Arg.Any<RotateLeftEventHandler>();
            playersInputDevice.Received().OnRotateRight += Arg.Any<RotateRightEventHandler>();
            playersInputDevice.Received().OnFire += Arg.Any<FireEventHandler>();
        }

        [Test]
        public void WhenDestroyedUnRegistersToEvents()
        {
            Initialize();

            Destroy();

            playersInputDevice.Received().OnFoward -= Arg.Any<FowardEventHandler>();
            playersInputDevice.Received().OnBackward -= Arg.Any<BackwardEventHandler>();
            playersInputDevice.Received().OnRotateLeft -= Arg.Any<RotateLeftEventHandler>();
            playersInputDevice.Received().OnRotateRight -= Arg.Any<RotateRightEventHandler>();
            playersInputDevice.Received().OnFire -= Arg.Any<FireEventHandler>();
        }

        [Test]
        public void CanConfigurePlayer()
        {
            Initialize();

            playerController.Configure();

            CheckHealthReset();
        }

        [Test]
        public void MoveFowardWhenAskedTo()
        {
            Initialize();

            PressMoveFowardInput();

            CheckMovedFoward();
        }

        [Test]
        public void MoveBackawrdWhenAskedTo()
        {
            Initialize();

            PressMoveBackwardInput();

            CheckMovedBackward();
        }

        [Test]
        public void RotateLeftWhenAskedTo()
        {
            Initialize();

            PressRotateLeftInput();

            CheckRotateLeft();
        }

        [Test]
        public void RotateRightWhenAskedTo()
        {
            Initialize();

            PressRotateRightInput();

            CheckRotateRight();
        }


        [Test]
        public void FireWhenAskedTo()
        {
            Initialize();

            PressFireInput();

            CheckFiredProjectile();
        }

        private void Initialize()
        {
            playerController.InjectPlayerController(health,
                                                    playerInputSensor,
                                                    physicsMover,
                                                    projectileShooter);
            playerController.Awake();
        }

        private void Destroy()
        {
            playerController.OnDestroy();
        }

        private void PressMoveFowardInput()
        {
            playersInputDevice.OnFoward += Raise.Event<FowardEventHandler>();
        }

        private void PressMoveBackwardInput()
        {
            playersInputDevice.OnBackward += Raise.Event<BackwardEventHandler>();
        }

        private void PressRotateLeftInput()
        {
            playersInputDevice.OnRotateLeft += Raise.Event<RotateLeftEventHandler>();
        }

        private void PressRotateRightInput()
        {
            playersInputDevice.OnRotateRight += Raise.Event<RotateRightEventHandler>();
        }

        private void PressFireInput()
        {
            playersInputDevice.OnFire += Raise.Event<FireEventHandler>();
        }

        private void CheckHealthReset()
        {
            health.Received().Reset();
        }

        private void CheckMovedFoward()
        {
            physicsMover.Received().AddFowardImpulse();
        }

        private void CheckMovedBackward()
        {
            physicsMover.Received().AddBackwardImpulse();
        }

        private void CheckRotateLeft()
        {
            physicsMover.Received().AddRotateLeftImpulse();
        }

        private void CheckRotateRight()
        {
            physicsMover.Received().AddRotateRightImpulse();
        }

        private void CheckFiredProjectile()
        {
            projectileShooter.Received().Fire();
        }
    }
}