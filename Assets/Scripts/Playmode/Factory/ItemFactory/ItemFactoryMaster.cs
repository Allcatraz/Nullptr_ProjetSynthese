using UnityEngine;
using UnityEngine.Networking;


namespace ProjetSynthese
{
    public class ItemFactoryMaster
    {
        [Command]
        public static GameObject CmdSpawnObject(Vector3 spawnPoint, GameObject prefab)
        {
            GameObject _object = Object.Instantiate(prefab, spawnPoint, Quaternion.identity);
            _object.transform.position += new Vector3(Random.value, 0, Random.value);
            //_object.transform.rotation = Random.rotation.y;

            NetworkServer.Spawn(_object);

            return _object;
        }
    }
}


