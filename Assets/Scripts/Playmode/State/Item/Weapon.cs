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

        public float EffectiveWeaponRange
        {
            get
            {
                return bulletLivingTime * bulletSpeed;
            }
        }

        public override void Use()
        {
            BulletFactory.CreateBullet(bulletSpawnPoint, bulletSpeed, bulletLivingTime);
        }
    }
}


