using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class AWMFabric : ItemFabricMaster
    {
        private static GameObject AWMPrefab;

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            GameObject _object = SpawnObject(spawnPoint, AWMPrefab);
        }
    }
}


