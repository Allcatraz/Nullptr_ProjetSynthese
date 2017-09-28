using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class AWMFabric : WeaponFabricMaster
    {
        public static GameObject AWMPrefab { get; set; }

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            SpawnWeapon(itemList, spawnPoint, rnd, AWMPrefab, AmmoType.AmmoWinchester);
        }
    }
}


