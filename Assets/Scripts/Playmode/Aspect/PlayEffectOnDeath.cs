using UnityEngine;
using Harmony;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/PlayEffectOnDeath")]
    public class PlayEffectOnDeath : GameScript
    {
        [SerializeField]
        private GameObject effectPrefab;

        private new ITransform transform;
        private IPrefabFactory prefabFactory;
        private Health health;

        public void InjectPlayEffectOnDeath(GameObject effectPrefab,
                                            [TopParentScope] ITransform transform,
                                            [ApplicationScope] IPrefabFactory prefabFactory,
                                            [EntityScope] Health health)
        {
            this.effectPrefab = effectPrefab;
            this.transform = transform;
            this.prefabFactory = prefabFactory;
            this.health = health;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayEffectOnDeath", effectPrefab);
        }

        public void OnEnable()
        {
            health.OnDeath += OnDeath;
        }

        public void OnDisable()
        {
            health.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            prefabFactory.Instantiate(effectPrefab, transform.Position, Quaternion.Euler(Vector3.zero));
        }
    }
}