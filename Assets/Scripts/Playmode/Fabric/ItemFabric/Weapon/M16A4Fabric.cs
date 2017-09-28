using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class M16A4Fabric : ItemFabricMaster
    {
        private static GameObject M16A4Prefab;

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            GameObject _object = SpawnObject(spawnPoint, M16A4Prefab);
        }
    }
}
