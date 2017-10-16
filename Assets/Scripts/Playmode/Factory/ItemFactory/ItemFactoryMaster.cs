using UnityEngine;
using UnityEngine.Networking;


namespace ProjetSynthese
{
    public class ItemFactoryMaster
    {
        [Command]
        public static GameObject CmdSpawnObject(GameObject spawnPoint, GameObject prefab)
        {
            GameObject _object = Object.Instantiate(prefab, spawnPoint.transform);
            _object.transform.position += new Vector3(Random.value, 0, Random.value);
            //_object.transform.rotation = Random.rotation.y;

            NetworkServer.Spawn(_object);

            return _object;
        }
    }
}


