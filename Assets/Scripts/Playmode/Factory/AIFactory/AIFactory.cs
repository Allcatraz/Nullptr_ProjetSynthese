using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class AIFactory
    {
        [Command]
        public static GameObject CmdSpawnAI( GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            return gameObject;
        }
    }
}
