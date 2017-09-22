using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/AddScoreOnPrefabDeath")]
    public class AddScoreOnPrefabDeath : GameScript
    {
        [SerializeField]
        private R.E.Prefab prefab;

        [SerializeField]
        private uint pointsPerPrefab;

        private Score score;
        private DeathEventChannel deathEventChannel;

        private void InjectAddScoreOnPrefabDeath([GameObjectScope] Score score,
                                                [EventChannelScope] DeathEventChannel deathEventChannel)
        {
            this.score = score;
            this.deathEventChannel = deathEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectAddScoreOnPrefabDeath");
        }

        private void OnEnable()
        {
            deathEventChannel.OnEventPublished += OnPrefabDeath;
        }

        private void OnDisable()
        {
            deathEventChannel.OnEventPublished -= OnPrefabDeath;
        }

        private void OnPrefabDeath(DeathEvent deathEvent)
        {
            if (deathEvent.DeadPrefab == prefab)
            {
                score.AddPoints(pointsPerPrefab);
            }
        }
    }
}