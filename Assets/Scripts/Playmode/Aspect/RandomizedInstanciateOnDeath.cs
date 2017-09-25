using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/RandomizedInstanciateOnDeath")]
    public class RandomizedInstanciateOnDeath : GameScript
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        [Range(0, 100)]
        private int chancesOfInstanciation;

        private Transform topParentTranform;
        private Health health;

        private void InjectRandomizedInstanciateOnDeath([TopParentScope] Transform topParentTranform,
                                                        [EntityScope] Health health)
        {
            this.topParentTranform = topParentTranform;
            this.health = health;
        }

        private void Awake()
        {
            InjectDependencies("InjectRandomizedInstanciateOnDeath");
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
            //Trigger it only when a random number is below the amount of chances of instanciation
            //
            //For example, when chances are 30%, we need the number to be bellow or equal to 30
            //    0       30                        100
            //    /===================================/
            //        ^                 ^
            //        |                 |
            //      Here            Not here

            if (RandomExtensions.GetRandomInt(0, 100) <= chancesOfInstanciation)
            {
                Instantiate(prefab, topParentTranform.position, Quaternion.Euler(Vector3.zero));
            }
        }
    }
}