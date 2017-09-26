using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Weapon : Item
    {
        [SerializeField]
        private GameObject bulletSpawnPoint;
        [SerializeField]
        private float bulletSpeed;
        [SerializeField]
        private float bulletLivingTime;

        public override void Use()
        {
            BulletFabric.CreateBullet(bulletSpawnPoint, bulletSpeed, bulletLivingTime);
        }
    }
}


