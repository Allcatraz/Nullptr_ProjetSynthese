using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : ActorController
    {
        private Health health;
        private KeyboardInputSensor playerInputSensor;
        private ImpulseMover impulseMover;

        private void InjectPlayerController([GameObjectScope] Health health, [ApplicationScope] KeyboardInputSensor playerInputSensor)
        {
            this.health = health;
            this.playerInputSensor = playerInputSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerController");

            playerInputSensor.Keyboards.OnFoward += OnFoward;
            playerInputSensor.Keyboards.OnBackward += OnBackward;
            playerInputSensor.Keyboards.OnRotateLeft += OnRotateLeft;
            playerInputSensor.Keyboards.OnRotateRight += OnRotateRight;

            playerInputSensor.Keyboards.OnFire += OnFire;
        }

        public void Configure()
        {
            health.Reset();
        }

        private void OnDestroy()
        {
            playerInputSensor.Keyboards.OnFoward -= OnFoward;
            playerInputSensor.Keyboards.OnBackward -= OnBackward;
            playerInputSensor.Keyboards.OnRotateLeft -= OnRotateLeft;
            playerInputSensor.Keyboards.OnRotateRight -= OnRotateRight;

            playerInputSensor.Keyboards.OnFire -= OnFire;
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
           
        }
    }
}