using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            Vector3 position = Vector3.zero;
            AIprefab.transform.position = position;
            
            for (int i = 0; i < AINumber; i++)
            {
                AIFabric.CmdSpawnAI(AIprefab);
            }
        }
    }
}