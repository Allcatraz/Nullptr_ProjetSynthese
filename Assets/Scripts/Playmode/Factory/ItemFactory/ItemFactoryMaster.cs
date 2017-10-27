using UnityEngine;
using UnityEngine.Networking;


namespace ProjetSynthese
{
    //BEN_CORRECTION : PrefabCreator ? NetworkPrefabCreator ?
    public class ItemFactoryMaster
    {
        //BEN_REVIEW : Partout ailleurs, le paramêtre "Prefab" vient avant la position. Juste le déplacer.
        //
        //             En passant, Resharper (ou même Visual Studio) peut vous aider à bouger les paramêtres
        //             sans avoir à tout changer vous même.
        public static GameObject SpawnObject(Vector3 spawnPoint, GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab, spawnPoint, Quaternion.identity);
            gameObject.transform.position += new Vector3(Random.value, 0, Random.value);

            return gameObject;
        }

        [Command]
        public static void CmdSpawnObject(GameObject obj)
        {
            NetworkServer.Spawn(obj);
        }
    }
}


