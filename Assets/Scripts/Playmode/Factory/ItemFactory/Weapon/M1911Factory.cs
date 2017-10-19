using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class M1911Factory : WeaponFactoryMaster
    {
        public static GameObject M1911Prefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random rnd)
        {
            SpawnWeapon(itemList, spawnPoint, rnd, M1911Prefab, AmmoType.Ammo45acp);
        }
    }
}
