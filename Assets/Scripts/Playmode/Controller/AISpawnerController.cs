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
            Vector3 position = new Vector3(200,10.0f,-200);
           
            AIprefab.GetComponent<NetworkStartPosition>().transform.position = position;
            for (int i = 0; i < AINumber; i++)
            {
                AIFactory.CmdSpawnAI(AIprefab);
            }
        }
    }
}