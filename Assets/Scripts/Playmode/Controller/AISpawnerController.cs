using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class AISpawnerController : NetworkGameScript
    {
        [Tooltip("AI prefab contenant les combatants AI")]
        [SerializeField]
        private GameObject aiPrefab;

       
        private const int AINumber = 1;

        private const float XMapCornerCoordinate = 0.0f;
        private const float ZMapCornerCoordinate = 0.0f;
        private const float DefaultHeighPosition = 1.0f;

        private const float XOffset = 15.0f;
        private const float ZOffset = -15.0f;

        private void Start()
        {
            SpawnAIs();
        }

        private void SpawnAIs()
        {
            Vector3[] position = new Vector3[AINumber];
            for (int i = 0; i < AINumber; i++)
            {
                position[i] = new Vector3(XMapCornerCoordinate + XOffset, DefaultHeighPosition, ZMapCornerCoordinate + ZOffset);
            }
    
            for (int i = 0; i < AINumber; i++)
            {
                aiPrefab.GetComponent<NetworkStartPosition>().transform.position = position[i];
                CmdSpawnObject(AIFactory.CmdSpawnAI(aiPrefab));
            }

            Destroy(this);
        }
    }
}