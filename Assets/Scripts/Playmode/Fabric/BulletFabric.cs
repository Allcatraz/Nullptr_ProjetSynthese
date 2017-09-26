using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public static class BulletFabric
    {
        public static GameObject BulletPrefab { get; set; }

        public static void CreateBullet(GameObject spawnPoint, float bulletSpeed, float livingTime)
        {
            GameObject bullet = Object.Instantiate(BulletPrefab);
            bullet.transform.position = spawnPoint.transform.position;
            bullet.transform.rotation = spawnPoint.transform.rotation;
            Vector3 direction = Vector3.Normalize(spawnPoint.transform.position - spawnPoint.transform.parent.position);
            Vector3 velocity = direction * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = velocity;
            bullet.GetComponent<BulletController>().SetLivingTime(livingTime);
        }
    }
}


