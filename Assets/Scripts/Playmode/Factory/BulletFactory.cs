using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public static class BulletFactory
    {
        public static GameObject BulletPrefab { get; set; }

        public static void CreateBullet(GameObject spawnPoint, float bulletSpeed, float livingTime)
        {
            GameObject bullet = Object.Instantiate(BulletPrefab);
            bullet.transform.position = spawnPoint.transform.position;
            bullet.transform.rotation = spawnPoint.transform.rotation;
            Vector3 direction = Vector3.Normalize(spawnPoint.transform.position - spawnPoint.transform.parent.position);
            Vector3 velocity = direction * bulletSpeed;
            bullet.GetComponent<Rigidbody>().velocity = velocity;
            bullet.GetComponent<BulletController>().SetLivingTime(livingTime);

            NetworkServer.Spawn(bullet);
        }
    }
}


