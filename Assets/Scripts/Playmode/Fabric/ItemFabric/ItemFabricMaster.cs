using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjetSynthese
{
    public class ItemFabricMaster
    {
        public static GameObject SpawnObject(GameObject spawnPoint, GameObject prefab)
        {
            GameObject _object = Object.Instantiate(prefab);
            _object.transform.position = spawnPoint.transform.position;
            _object.transform.rotation = Random.rotation;
            return _object;
        }
    }
}


