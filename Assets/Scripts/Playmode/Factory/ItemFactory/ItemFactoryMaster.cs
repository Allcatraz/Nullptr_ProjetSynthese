using UnityEngine;
using UnityEngine.Networking;


namespace ProjetSynthese
{
    public class ItemFactoryMaster
    {
        public static GameObject SpawnObject(Vector3 spawnPoint, GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab, spawnPoint, Quaternion.identity);
            gameObject.transform.position += new Vector3(Random.value, 0, Random.value);

            NetworkServer.Spawn(gameObject);

            return gameObject;
        }

        [Command]
        public static void CmdSpawnObject(GameObject obj)
        {
            NetworkServer.Spawn(obj);
        }
    }
}


