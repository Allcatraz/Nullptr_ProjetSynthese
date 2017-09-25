using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Weapon : Item
    {
        [SerializeField]
        GameObject bulletSpawnPoint;
        [SerializeField]
        float BulletSpeed;

        public override void Use()
        {
            BulletFabric.CreateBullet(bulletSpawnPoint, BulletSpeed);
        }
    }
}


