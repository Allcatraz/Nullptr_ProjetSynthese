using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class SAIGA12Fabric : ItemFabricMaster
    {
        private static GameObject SAIGA12Prefab;

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            GameObject _object = SpawnObject(spawnPoint, SAIGA12Prefab);
        }
    }
}
