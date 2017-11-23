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
        private const float XMapOriginOppositeCornerCoordinate = 50.0f;//40000.0f;
        private const float ZMapOriginOppositeCornerCoordinate = 50.0f;//40000.0f;

        private void Start()
        {

            SpawnAIs();
        }

        private void SpawnAIs()
        {
            Vector3[] position = new Vector3[AINumber];
            float xOffset = 0.0f;
            float zOffset = 0.0f;
            for (int i = 0; i < AINumber; i++)
            {
                xOffset = Random.Range(XMapOriginCornerCoordinate, XMapOriginOppositeCornerCoordinate);
                zOffset = Random.Range(ZMapOriginCornerCoordinate, ZMapOriginOppositeCornerCoordinate);
                position[i] = new Vector3(XMapOriginCornerCoordinate + xOffset, DefaultHeighPosition, ZMapOriginOppositeCornerCoordinate + zOffset);
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