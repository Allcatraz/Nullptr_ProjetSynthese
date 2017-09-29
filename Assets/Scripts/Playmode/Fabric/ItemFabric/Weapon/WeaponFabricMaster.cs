using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class WeaponFabricMaster : ItemFabricMaster
    {
        protected static void SpawnWeapon(List<Item> itemList, GameObject spawnPoint, System.Random rnd, GameObject weaponPrefab, AmmoType ammoType)
        {
            GameObject _object = SpawnObject(spawnPoint, weaponPrefab);
            Weapon w = _object.GetComponent<Weapon>();
            itemList.Add(w);
            AmmoFabric.CreateItem(itemList, spawnPoint, rnd, ammoType);
        }
    }
}


