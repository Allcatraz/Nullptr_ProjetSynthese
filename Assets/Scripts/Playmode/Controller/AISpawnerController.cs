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
        private const float DefaultHeighPosition = 2.0f;
        private const float XMapOriginOppositeCornerCoordinate = 50.0f;//40000.0f;
        private const float ZMapOriginOppositeCornerCoordinate = -50.0f;//40000.0f;

        private static int timeExecute = 0;

        private void Start()
        {
            SpawnAIs();
        }

        private void SpawnAIs()
        {
            if (timeExecute < 1)
            {
                Vector3[] position = new Vector3[AINumber];
                float xOffset = 0.0f;
                float zOffset = 0.0f;
                for (int i = 0; i < AINumber; i++)
                {
                    xOffset = Random.Range(XMapOriginCornerCoordinate, XMapOriginOppositeCornerCoordinate);
                    zOffset = Random.Range(ZMapOriginCornerCoordinate, ZMapOriginOppositeCornerCoordinate);
                    position[i] = new Vector3(XMapOriginCornerCoordinate + xOffset, DefaultHeighPosition, ZMapOriginCornerCoordinate + zOffset);
                }

                for (int i = 0; i < AINumber; i++)
                {
                    CmdSpawnAi(position[i]);
                }

                timeExecute++;
                Destroy(this);
            }
            else
            {
                Destroy(this);
            }
        }

        [Command]
        private void CmdSpawnAi(Vector3 position)
        {
            GameObject aI = Instantiate(aiPrefab);
            aI.GetComponent<NetworkStartPosition>().transform.position = position;
            NetworkServer.Spawn(aI);
        }
    }
}