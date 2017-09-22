using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : GameScript
    {
        private Health health;
        private PlayerInputSensor playerInputSensor;
        private ImpulseMover impulseMover;
        private ProjectileShooter projectileShooter;

        private void InjectPlayerController([GameObjectScope] Health health,
                                           [ApplicationScope] PlayerInputSensor playerInputSensor,
                                           [EntityScope] ImpulseMover impulseMover,
                                           [EntityScope] ProjectileShooter projectileShooter)
        {
            this.health = health;
            this.playerInputSensor = playerInputSensor;
            this.impulseMover = impulseMover;
            this.projectileShooter = projectileShooter;
        }

        private void Awake()
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

        private void OnDestroy()
        {
            playerInputSensor.Players.OnFoward -= OnFoward;
            playerInputSensor.Players.OnBackward -= OnBackward;
            playerInputSensor.Players.OnRotateLeft -= OnRotateLeft;
            playerInputSensor.Players.OnRotateRight -= OnRotateRight;

            playerInputSensor.Players.OnFire -= OnFire;
        }

        private void OnFoward()
        {
            impulseMover.AddFowardImpulse();
        }

        private void OnBackward()
        {
            impulseMover.AddBackwardImpulse();
        }

        private void OnRotateLeft()
        {
            impulseMover.AddRotateLeftImpulse();
        }

        private void OnRotateRight()
        {
            impulseMover.AddRotateRightImpulse();
        }

        private void OnFire()
        {
            projectileShooter.Fire();
        }
    }
}