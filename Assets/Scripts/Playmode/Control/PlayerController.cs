using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Control/PlayerController")]
    public class PlayerController : GameScript
    {
        private Health health;
        private PlayerInputSensor playerInputSensor;
        private PhysicsMover physicsMover;
        private ProjectileShooter projectileShooter;

        public void InjectPlayerController([GameObjectScope] Health health,
                                           [ApplicationScope] PlayerInputSensor playerInputSensor,
                                           [EntityScope] PhysicsMover physicsMover,
                                           [EntityScope] ProjectileShooter projectileShooter)
        {
            this.health = health;
            this.playerInputSensor = playerInputSensor;
            this.physicsMover = physicsMover;
            this.projectileShooter = projectileShooter;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayerController");

            playerInputSensor.Players.OnFoward += OnFoward;
            playerInputSensor.Players.OnBackward += OnBackward;
            playerInputSensor.Players.OnRotateLeft += OnRotateLeft;
            playerInputSensor.Players.OnRotateRight += OnRotateRight;

            playerInputSensor.Players.OnFire += OnFire;
        }

        public void Configure()
        {
            health.Reset();
        }

        public void OnDestroy()
        {
            playerInputSensor.Players.OnFoward -= OnFoward;
            playerInputSensor.Players.OnBackward -= OnBackward;
            playerInputSensor.Players.OnRotateLeft -= OnRotateLeft;
            playerInputSensor.Players.OnRotateRight -= OnRotateRight;

            playerInputSensor.Players.OnFire -= OnFire;
        }

        private void OnFoward()
        {
            physicsMover.AddFowardImpulse();
        }

        private void OnBackward()
        {
            physicsMover.AddBackwardImpulse();
        }

        private void OnRotateLeft()
        {
            physicsMover.AddRotateLeftImpulse();
        }

        private void OnRotateRight()
        {
            physicsMover.AddRotateRightImpulse();
        }

        private void OnFire()
        {
            projectileShooter.Fire();
        }
    }
}