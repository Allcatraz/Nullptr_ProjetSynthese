using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/DeathCircleDistanceEventPublisher")]
    public class DeathCircleDistanceEventPublisher : GameScript
    {
        private DeathCircleController deathCircleController;
        private DeathCircleDistanceEventChannel deathCircleDistanceEventChannel;

        private void InjectDeathCircleDistanceEventPublisher([GameObjectScope] DeathCircleController deathCircleController,
                                                             [EventChannelScope] DeathCircleDistanceEventChannel deathCircleDistanceEventChannel)
        {
            this.deathCircleDistanceEventChannel = deathCircleDistanceEventChannel;
            this.deathCircleController = deathCircleController;
        }

        // Use this for initialization
        private void Awake()
        {
            InjectDependencies("InjectDeathCircleDistanceEventPublisher");
        }

        private void OnEnable()
        {
            deathCircleController.OnDistanceChanged += OnDistanceChanged;
        }

        private void OnDisable()
        {
            deathCircleController.OnDistanceChanged -= OnDistanceChanged;
        }

        private void OnDistanceChanged(float safeCircleRadius, float deathCircleRadius, Vector3 center)
        {
            deathCircleDistanceEventChannel.Publish(new DeathCircleDistanceEvent(safeCircleRadius, deathCircleRadius, center));
        }
    }
}