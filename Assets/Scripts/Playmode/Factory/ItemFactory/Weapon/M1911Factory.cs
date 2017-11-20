using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class M1911Factory : WeaponFactoryMaster
    {
        public static GameObject M1911Prefab { get; set; }

        public static void CreateItem(List<GameObject> itemList, Vector3 spawnPoint, System.Random random)
        {
            SpawnWeapon(itemList, spawnPoint, random, M1911Prefab, AmmoType.Ammo9mm);
        }
    }
}
