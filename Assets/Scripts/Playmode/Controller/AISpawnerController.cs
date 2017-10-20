using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class AISpawnerController : MonoBehaviour
    {
        [SerializeField]
        private GameObject AIprefab;

        [SerializeField]
        const int AINumber = 1;
        void Start()
        {
            SpawnAIs();
        }

        private void SpawnAIs()
        {
            Vector3[] position = new Vector3[2];
            position[0] = new Vector3(15, 1.0f, -15);
            position[1] = new Vector3(20, 1.0f, -20);
            
            for (int i = 0; i < AINumber; i++)
            {
                AIprefab.GetComponent<NetworkStartPosition>().transform.position = position[i];
                AIFactory.CmdSpawnAI(AIprefab);
            }
        }
    }
}