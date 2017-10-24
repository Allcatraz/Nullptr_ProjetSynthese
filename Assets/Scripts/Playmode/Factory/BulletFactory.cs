using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public static class BulletFactory
    {
        public static GameObject BulletPrefab { get; set; }

        [Command]
        public static void CmdCreateBullet(GameObject spawnPoint, GameObject chamber, float bulletSpeed, float livingTime)
        {
            GameObject bullet = Object.Instantiate(BulletPrefab);
            bullet.transform.position = spawnPoint.transform.position;
            bullet.transform.rotation = spawnPoint.transform.rotation;
            Vector3 direction = Vector3.Normalize(spawnPoint.transform.position - chamber.transform.position);
            Vector3 velocity = direction * bulletSpeed;
            bullet.GetComponent<Rigidbody>().velocity = velocity;

            NetworkServer.Spawn(bullet);

            bullet.GetComponent<BulletController>().SetLivingTime(livingTime);
        }
    }
}


