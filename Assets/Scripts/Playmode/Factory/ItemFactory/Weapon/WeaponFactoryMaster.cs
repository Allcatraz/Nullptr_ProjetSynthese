using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class WeaponFactoryMaster : ItemFactoryMaster
    {
        protected static void SpawnWeapon(List<GameObject> itemList, Vector3 spawnPoint, System.Random random, GameObject weaponPrefab, AmmoType ammoType)
        {
            GameObject gameObject = SpawnObject(spawnPoint, weaponPrefab);
            itemList.Add(gameObject);
            AmmoFactory.CreateItem(itemList, spawnPoint, random, ammoType);
        }

        protected static void SpawnWeapon(List<GameObject> itemList, Vector3 spawnPoint, System.Random random, GameObject weaponPrefab)
        {
            GameObject gameObject = SpawnObject(spawnPoint, weaponPrefab);
            itemList.Add(gameObject);
        }
    }
}
