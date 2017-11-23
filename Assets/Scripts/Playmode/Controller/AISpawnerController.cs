using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class AISpawnerController : NetworkGameScript
    {
        [Tooltip("AI prefab contenant les combatants AI")]
        [SerializeField]
        private GameObject aiPrefab;


        private const int AINumber = 5;

        private const float XMapOriginCornerCoordinate = 0.0f;
        private const float ZMapOriginCornerCoordinate = 0.0f;
        private const float DefaultHeighPosition = 1.0f;
        private const float XMapOriginOppositeCornerCoordinate = 40000.0f;
        private const float ZMapOriginOppositeCornerCoordinate = 40000.0f;

        private void Start()
        {

            SpawnAIs();
        }

        private void SpawnAIs()
        {
            Vector3[] position = new Vector3[AINumber];
            float XOffset = 0.0f;
            float ZOffset = 0.0f;
            for (int i = 0; i < AINumber; i++)
            {
                XOffset = Random.Range(XMapOriginCornerCoordinate, XMapOriginOppositeCornerCoordinate);
                ZOffset = Random.Range(ZMapOriginCornerCoordinate, ZMapOriginOppositeCornerCoordinate);
                position[i] = new Vector3(XMapOriginCornerCoordinate + XOffset, DefaultHeighPosition, ZMapOriginOppositeCornerCoordinate + ZOffset);
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