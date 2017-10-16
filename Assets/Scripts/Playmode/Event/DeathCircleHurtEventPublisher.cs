using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/DeathCircleHurtEventPublisher")]
    public class DeathCircleHurtEventPublisher : GameScript
    {
        private DeathCircleController deathCircle;
        private DeathCircleHurtEventChannel deathCircleHurtEventChannel;

        private void InjectDeathCircleHurtEventPublisher([GameObjectScope] DeathCircleController deathCircle,
                                                         [EventChannelScope] DeathCircleHurtEventChannel deathCircleHurtEventChannel)
        {
            this.deathCircle = deathCircle;
            this.deathCircleHurtEventChannel = deathCircleHurtEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDeathCircleHurtEventPublisher");
        }

        private void OnEnable()
        {
            deathCircle.OnPlayerHurt += OnHurtPlayer;
        }

        private void OnDisable()
        {
            deathCircle.OnPlayerHurt -= OnHurtPlayer;
        }

        private void OnHurtPlayer(float hurtPoints)
        {
            deathCircleHurtEventChannel.Publish(new DeathCircleHurtEvent(hurtPoints));
        }
    }
}
