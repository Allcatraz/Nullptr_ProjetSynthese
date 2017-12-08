using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class AISpawnerController : NetworkGameScript
    {
        [Tooltip("AI prefab contenant les combatants AI")]
        [SerializeField]
        private GameObject aiPrefab;

        private const int AINumber = 10;

        public const float XMapOriginCornerCoordinate = 150.0f;//0.0f;
        public const float ZMapOriginCornerCoordinate = -150.0f;//0.0f;
        private const float DefaultHeighPosition = 2.0f;
        public const float XMapOriginOppositeCornerCoordinate = 550.0f;//450.0f;
        public const float ZMapOriginOppositeCornerCoordinate = -550.0f;//-450.0f;

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
                    position[i] = new Vector3(xOffset, DefaultHeighPosition, zOffset);
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