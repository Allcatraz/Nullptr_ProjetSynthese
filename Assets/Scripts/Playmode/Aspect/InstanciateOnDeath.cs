using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/InstanciateOnDeath")]
    public class InstanciateOnDeath : GameScript
    {
        [SerializeField]
        private GameObject prefab;

        private Transform topParentTranform;
        private Health health;

        private void InjectInstanciateOnDeath([TopParentScope] Transform topParentTranform,
                                            [EntityScope] Health health)
        {
            this.topParentTranform = topParentTranform;
            this.health = health;
        }

        private void Awake()
        {
            InjectDependencies("InjectInstanciateOnDeath");
        }

        private void OnEnable()
        {
            health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            health.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            Instantiate(prefab, topParentTranform.position, Quaternion.Euler(Vector3.zero));
        }
    }
}