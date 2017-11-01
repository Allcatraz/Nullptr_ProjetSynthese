using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class SAIGA12Factory : WeaponFactoryMaster
    {
        public static GameObject Saiga12Prefab { get; set; }

        public static void CreateItem(List<GameObject> itemList, Vector3 spawnPoint, System.Random random)
        {
            SpawnWeapon(itemList, spawnPoint, random, Saiga12Prefab, AmmoType.AmmoCalibre12);
        }
    }
}
