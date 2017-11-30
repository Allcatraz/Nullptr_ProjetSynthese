using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class AWMFactory : WeaponFactoryMaster
    {
        public static GameObject AWMPrefab { get; set; }

        public static void CreateItem(List<GameObject> itemList, Vector3 spawnPoint, System.Random random)
        {
            SpawnWeapon(itemList, spawnPoint, random, AWMPrefab, AmmoType.Ammo762);
        }
    }
}
