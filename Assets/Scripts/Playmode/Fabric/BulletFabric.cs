using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public static class BulletFabric
    {
        public static GameObject BulletPrefab { get; set; }


        public static void CreateBullet(GameObject spawnPoint)
        {
            GameObject bullet = Object.Instantiate(BulletPrefab);
            UnityEngine.Networking.NetworkServer.Spawn(bullet);
        }
    }
}


