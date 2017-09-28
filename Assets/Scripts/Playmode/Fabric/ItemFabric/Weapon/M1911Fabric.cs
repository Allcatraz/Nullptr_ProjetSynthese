using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class M1911Fabric : ItemFabricMaster
    {
        private static GameObject M1911Prefab;

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            GameObject _object = SpawnObject(spawnPoint, M1911Prefab);
        }
    }
}
