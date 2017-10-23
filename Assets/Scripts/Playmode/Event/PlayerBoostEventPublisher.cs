using Harmony;

namespace ProjetSynthese
{
    public class PlayerBoostEventPublisher : GameScript
    {
        private BoostStats boostStats;
        private PlayerBoostEventChannel playerBoostEventChannel;

        private void InjectPlayerBoostEventPublisher([GameObjectScope] BoostStats boostStats,
                                                     [EventChannelScope] PlayerBoostEventChannel playerBoostEventChannel)
        {
            this.boostStats = boostStats;
            this.playerBoostEventChannel = playerBoostEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerBoostEventPublisher");
        }

        private void OnEnable()
        {
            boostStats.OnBoostChanged += OnBoostChanged;
        }

        private void OnDisable()
        {
            boostStats.OnBoostChanged -= OnBoostChanged;
        }

        private void OnBoostChanged(float oldBoostPoints, float newBoostPoints)
        {
            playerBoostEventChannel.Publish(new PlayerBoostEvent(boostStats, oldBoostPoints, newBoostPoints));
        }

    }
}
