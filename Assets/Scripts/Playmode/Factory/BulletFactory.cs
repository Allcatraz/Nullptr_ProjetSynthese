using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public static class BulletFactory
    {
        public static GameObject BulletPrefab { get; set; }

        public static GameObject CreateBullet(GameObject spawnPoint, GameObject chamber, float bulletSpeed, float livingTime, float dommage)
        {
            GameObject bullet = Object.Instantiate(BulletPrefab);
            bullet.transform.position = spawnPoint.transform.position;
            bullet.transform.rotation = spawnPoint.transform.rotation;

            Vector3 direction = Vector3.Normalize(spawnPoint.transform.position - chamber.transform.position);
            Vector3 velocity = direction * bulletSpeed;
            bullet.GetComponent<Rigidbody>().velocity = velocity;

            BulletController controller = bullet.GetComponent<BulletController>();
            controller.SetLivingTime(livingTime);
            controller.SetDommage(dommage);

            return bullet;
        }
    }
}
