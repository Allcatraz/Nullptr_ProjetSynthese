using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class M16A4Fabric : WeaponFabricMaster
    {
        public static GameObject M16A4Prefab { get; set; }

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            SpawnWeapon(itemList, spawnPoint, rnd, M16A4Prefab, AmmoType.Ammo556);
        }
    }
}
