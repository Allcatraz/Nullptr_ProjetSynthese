using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class WeaponFactoryMaster : ItemFactoryMaster
    {
        protected static void SpawnWeapon(List<Item> itemList, GameObject spawnPoint, System.Random rnd, GameObject weaponPrefab, AmmoType ammoType)
        {
            GameObject _object = CmdSpawnObject(spawnPoint, weaponPrefab);
            Weapon w = _object.GetComponent<Weapon>();
            itemList.Add(w);
            AmmoFactory.CreateItem(itemList, spawnPoint, rnd, ammoType);
        }
    }
}


