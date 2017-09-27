using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : ActorController
    {
        private Health health;
        private KeyboardInputSensor keyboardInputSensor;
        private MouseInputSensor mouseInputSensor;
        private PlayerMover playerMover;

        private void InjectPlayerController([ApplicationScope] KeyboardInputSensor keyboardInputSensor,
                                            [ApplicationScope] MouseInputSensor mouseInputSensor,
                                            [GameObjectScope] PlayerMover playerMover,
                                            [GameObjectScope] Health health)
        {
            this.mouseInputSensor = mouseInputSensor;
            this.keyboardInputSensor = keyboardInputSensor;
            this.playerMover = playerMover;
            this.health = health;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerController");
            keyboardInputSensor.Keyboards.OnMove += OnMove;
            keyboardInputSensor.Keyboards.OnFire += OnFire;
        }

        private void OnDestroy()
        {
            keyboardInputSensor.Keyboards.OnMove -= OnMove;
            keyboardInputSensor.Keyboards.OnFire -= OnFire;
        }

        private void Update()
        {
            playerMover.Rotate();
        }

        private void OnMove(Vector3 direction)
        {
            playerMover.Move(direction);
        }

        private void OnFire()
        {

        }
    }
}
