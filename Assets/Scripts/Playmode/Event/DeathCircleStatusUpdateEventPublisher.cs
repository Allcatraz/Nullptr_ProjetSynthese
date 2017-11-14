using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/DeathCircleStatusUpdateEventPublisher")]
    public class DeathCircleStatusUpdateEventPublisher : GameScript
    {
        private DeathCircleController deathCircleController;
        private DeathCircleStatusUpdateEventChannel deathCircleStatusUpdateEventChannel;

        private void InjectDeathCircleStatusUpdateEventPublisher([EntityScope] DeathCircleController deathCircleController,
                                                    [EventChannelScope] DeathCircleStatusUpdateEventChannel deathCircleStatusUpdateEventChannel)
        {
            this.deathCircleController = deathCircleController;
            this.deathCircleStatusUpdateEventChannel = deathCircleStatusUpdateEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDeathCircleStatusUpdateEventPublisher");
        }

        private void OnEnable()
        {
            deathCircleController.OnFixedUpdate += OnFixedUpdateCall;
        }

        private void OnDisable()
        {
            deathCircleController.OnFixedUpdate -= OnFixedUpdateCall;
        }

        private void OnFixedUpdateCall()
        {
            deathCircleStatusUpdateEventChannel.Publish(new DeathCircleStatusUpdateEvent(deathCircleController));
        }
    }
}