﻿using UnityEngine;
using UnityEngine.Networking;


namespace ProjetSynthese
{
    public class ItemFactoryMaster
    {
        public static GameObject SpawnObject(Vector3 spawnPoint, GameObject prefab)
        {
            GameObject _object = Object.Instantiate(prefab, spawnPoint, Quaternion.identity);
            _object.transform.position += new Vector3(Random.value, 0, Random.value);
            return _object;
        }

        [Command]
        public static void CmdSpawnObject(GameObject obj)
        {
            NetworkServer.Spawn(obj);
        }
    }
}


