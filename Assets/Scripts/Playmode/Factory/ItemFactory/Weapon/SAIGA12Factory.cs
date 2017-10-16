using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class SAIGA12Factory : WeaponFactoryMaster
    {
        public static GameObject SAIGA12Prefab { get; set; }

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            SpawnWeapon(itemList, spawnPoint, rnd, SAIGA12Prefab, AmmoType.AmmoCalibre12);
        }
    }
}
