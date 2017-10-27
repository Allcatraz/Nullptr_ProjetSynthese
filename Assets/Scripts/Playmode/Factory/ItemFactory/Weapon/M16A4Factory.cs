using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class M16A4Factory : WeaponFactoryMaster
    {
        public static GameObject M16A4Prefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random)
        {
            SpawnWeapon(itemList, spawnPoint, random, M16A4Prefab, AmmoType.Ammo556);
        }
    }
}
