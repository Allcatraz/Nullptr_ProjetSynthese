using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class AISpawnerController : MonoBehaviour
    {
        [Tooltip("AI prefab contenant les combatants AI")]
        [SerializeField]
        private GameObject AIprefab;

        [Tooltip("Nombre de combatants AI")]
        [SerializeField]
        const int AINumber = 1;

        const float XMapCornerCoordinate = 0.0f;
        const float ZMapCornerCoordinate = 0.0f;
        const float DefaultHeighPosition = 1.0f;

        const float XOffset = 15.0f;
        const float ZOffset = -15.0f;

        void Start()
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
                AIprefab.GetComponent<NetworkStartPosition>().transform.position = position[i];
                AIFactory.CmdSpawnAI(AIprefab);
            }
        }
    }
}