using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Control/RockController")]
    public class RockController : GameScript
    {
        [Range(0, 10)]
        [SerializeField]
        private uint minRockFragmentsOnSplit = 2;

        [Range(0, 10)]
        [SerializeField]
        private uint maxRockFragmentsOnSplit = 5;

        private Transform topParentTransform;
        private Health health;
        private RandomRockShape randomRockShape;
        private ImpulseMover impulseMover;
        private RockSpawner rockSpawner;

        private void InjectRockController([TopParentScope] Transform topParentTransform,
                                         [EntityScope] Health health,
                                         [EntityScope] RandomRockShape randomRockShape,
                                         [EntityScope] ImpulseMover impulseMover,
                                         [SceneScope] RockSpawner rockSpawner)
        {
            this.topParentTransform = topParentTransform;
            this.health = health;
            this.randomRockShape = randomRockShape;
            this.impulseMover = impulseMover;
            this.rockSpawner = rockSpawner;
        }

        private void Awake()
        {
            InjectDependencies("InjectRockController");

            health.OnDeath += OnDeath;
        }

        public void Configure(float radius, Vector3 direction, float force)
        {
            randomRockShape.Radius = radius;
            impulseMover.AddImpulse(direction, force);
        }

        private void OnDestroy()
        {
            health.OnDeath -= OnDeath;
        }

        private void Explode()
        {
            rockSpawner.SpawnFragments(RandomExtensions.GetRandomUInt(minRockFragmentsOnSplit, maxRockFragmentsOnSplit),
                                       topParentTransform.position,
                                       randomRockShape.Radius);
        }

        private void OnDeath()
        {
            Explode();
        }
    }
}