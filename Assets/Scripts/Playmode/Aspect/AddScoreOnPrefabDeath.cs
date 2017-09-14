using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Aspect/AddScoreOnPrefabDeath")]
    public class AddScoreOnPrefabDeath : GameScript
    {
        [SerializeField]
        private R.E.Prefab prefab;

        [SerializeField]
        private uint pointsPerPrefab;

        private Score score;
        private DeathEventChannel deathEventChannel;


        public void InjectAddScoreOnPrefabDeath(R.E.Prefab prefab,
                                                uint pointsPerPrefab,
                                                [GameObjectScope] Score score,
                                                [EventChannelScope] DeathEventChannel deathEventChannel)
        {
            this.prefab = prefab;
            this.pointsPerPrefab = pointsPerPrefab;
            this.score = score;
            this.deathEventChannel = deathEventChannel;
        }


        public void Awake()
        {
            InjectDependencies("InjectAddScoreOnPrefabDeath",
                               prefab,
                               pointsPerPrefab);
        }

        public void OnEnable()
        {
            deathEventChannel.OnEventPublished += OnPrefabDeath;
        }

        public void OnDisable()
        {
            deathEventChannel.OnEventPublished -= OnPrefabDeath;
        }

        //Needed for tests, when this class is mocked. Calling "enabled" while in tests causes a NullReferenceException.
        public virtual void EnableScoreCount()
        {
            enabled = true;
        }

        //Needed for tests, when this class is mocked. Calling "enabled" while in tests causes a NullReferenceException.
        public virtual void DisableScoreCount()
        {
            enabled = false;
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