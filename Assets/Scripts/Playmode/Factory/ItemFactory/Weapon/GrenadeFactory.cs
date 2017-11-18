using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class GrenadeFactory : WeaponFactoryMaster
    {
        public static GameObject GrenadePrefab{ get; set; }

        public static void CreateItem(List<GameObject> itemList, Vector3 spawnPoint, System.Random random)
        {
            SpawnWeapon(itemList, spawnPoint, random, GrenadePrefab);
        }
    }
}


