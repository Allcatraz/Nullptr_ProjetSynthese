using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/DeathCircleTimeLeftEventPublisher")]
    public class DeathCircleTimeLeftEventPublisher : GameScript
    {
        private DeathCircleController deathCircleController;
        private DeathCircleTimeLeftEventChannel deathCircleTimeLeftEventChannel;

        private void InjectDeathCircleTimeLeftEventPublisher([GameObjectScope] DeathCircleController deathCircleController,
                                                             [EventChannelScope] DeathCircleTimeLeftEventChannel deathCircleTimeLeftEventChannel)
        {
            this.deathCircleController = deathCircleController;
            this.deathCircleTimeLeftEventChannel = deathCircleTimeLeftEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDeathCircleTimeLeftEventPublisher");    
        }

        private void OnEnable()
        {
            deathCircleController.OnTimeLeft += OnTimeLeft;
        }

        private void OnDisable()
        {
            deathCircleController.OnTimeLeft -= OnTimeLeft;
        }

        private void OnTimeLeft(int minutes, int seconds, bool isWaitFinish)
        {
            deathCircleTimeLeftEventChannel.Publish(new DeathCircleTimeLeftEvent(minutes, seconds, isWaitFinish));
        }
    }
}