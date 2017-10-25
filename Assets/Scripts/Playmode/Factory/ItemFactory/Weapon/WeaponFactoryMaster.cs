using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class WeaponFactoryMaster : ItemFactoryMaster
    {
        protected static void SpawnWeapon(List<Item> itemList, Vector3 spawnPoint, System.Random rnd, GameObject weaponPrefab, AmmoType ammoType)
        {
            GameObject gameObject = SpawnObject(spawnPoint, weaponPrefab);
            Weapon w = gameObject.GetComponent<Weapon>();
            itemList.Add(w);
            AmmoFactory.CreateItem(itemList, spawnPoint, rnd, ammoType);
            CmdSpawnObject(gameObject);
        }
    }
}


