﻿using UnityEngine;
using UnityEngine.Networking;


namespace ProjetSynthese
{
    public class AIFactory
    {
        [Command]
        public static GameObject CmdSpawnAI( GameObject prefab)
        {
            GameObject _object = Object.Instantiate(prefab);
            
            NetworkServer.Spawn(_object);

            return _object;
        }
    }
}

