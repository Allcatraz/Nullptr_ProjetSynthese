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
            _object.transform.position = spawnPoint.transform.position + new Vector3(Random.value, 0, Random.value);
            //_object.transform.rotation = Random.rotation;
            return _object;
        }
    }
}


