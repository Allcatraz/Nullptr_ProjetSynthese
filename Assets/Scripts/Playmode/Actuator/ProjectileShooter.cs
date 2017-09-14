using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Actuator/ProjectileShooter")]
    public class ProjectileShooter : GameScript
    {
        [SerializeField]
        private GameObject projectilePrefab;

        private new ITransform transform;
        private IPrefabFactory prefabFactory;

        public void InjectShipVehiculeFire(GameObject projectilePrefab,
                                           [GameObjectScope] ITransform transform,
                                           [ApplicationScope] IPrefabFactory prefabFactory)
        {
            this.projectilePrefab = projectilePrefab;
            this.transform = transform;
            this.prefabFactory = prefabFactory;
        }

        public void Awake()
        {
            InjectDependencies("InjectShipVehiculeFire", projectilePrefab);
        }

        public virtual void Fire()
        {
            prefabFactory.Instantiate(projectilePrefab, transform.Position, transform.Rotation);
        }
    }
}