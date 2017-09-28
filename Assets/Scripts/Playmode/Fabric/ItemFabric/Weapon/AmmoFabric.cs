using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class AmmoFabric : ItemFabricMaster
    {

        private static GameObject ammoPackPrefab;

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd, AmmoType ammoType)
        {
            GameObject _object = SpawnObject(spawnPoint, ammoPackPrefab);
        }
    }
}


