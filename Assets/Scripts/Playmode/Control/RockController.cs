using Harmony;
using UnityEngine;
using Harmony.Injection;

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

        private new ITransform transform;
        private IRandom random;
        private Health health;
        private RandomRockShape randomRockShape;
        private PhysicsMover physicsMover;
        private RockSpawner rockSpawner;

        public void InjectRockController(uint minRockFragmentsOnSplit,
                                         uint maxRockFragmentsOnSplit,
                                         [TopParentScope] ITransform transform,
                                         [ApplicationScope] IRandom random,
                                         [EntityScope] Health health,
                                         [EntityScope] RandomRockShape randomRockShape,
                                         [EntityScope] PhysicsMover physicsMover,
                                         [SceneScope] RockSpawner rockSpawner)
        {
            this.minRockFragmentsOnSplit = minRockFragmentsOnSplit;
            this.maxRockFragmentsOnSplit = maxRockFragmentsOnSplit;
            this.transform = transform;
            this.random = random;
            this.health = health;
            this.randomRockShape = randomRockShape;
            this.physicsMover = physicsMover;
            this.rockSpawner = rockSpawner;
        }

        public void Awake()
        {
            InjectDependencies("InjectRockController",
                               minRockFragmentsOnSplit,
                               maxRockFragmentsOnSplit);

            health.OnDeath += OnDeath;
        }

        public void Configure(float radius, Vector3 direction, float force)
        {
            randomRockShape.Radius = radius;
            physicsMover.AddImpulse(direction, force);
        }

        public void OnDestroy()
        {
            health.OnDeath -= OnDeath;
        }

        private void Explode()
        {
            rockSpawner.SpawnFragments(random.GetRandomUInt(minRockFragmentsOnSplit, maxRockFragmentsOnSplit),
                                       transform.Position,
                                       randomRockShape.Radius);
        }

        private void OnDeath()
        {
            Explode();
        }
    }
}