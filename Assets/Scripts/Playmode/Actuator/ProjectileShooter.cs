using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/ProjectileShooter")]
    public class ProjectileShooter : GameScript
    {
        [SerializeField]
        private GameObject projectilePrefab;

        public void Fire()
        {
            Instantiate(projectilePrefab, transform.position, transform.rotation);
        }
    }
}