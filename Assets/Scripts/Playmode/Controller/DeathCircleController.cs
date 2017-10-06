using Harmony;
using Tiled2Unity;
using UnityEngine;


namespace ProjetSynthese
{
    public class DeathCircleController : GameScript
    {
        enum Phases { Phase1, Phase2, Phase3, Phase4, Phase5, Phase6, Phase7, Phase8 }

        [SerializeField] private DeathCircle deathCircleValues;

        private const float InitialShrinkTime = 0.05f;
        private float waitTime;
        private float moveTime;
        private float shrinkTime = InitialShrinkTime;
        private bool hasMoveTimeSet = false;

        private Torus safeCircle;
        private Torus deathCircle;
        private TiledMap tiledMap;
        private Phases currentPhase = Phases.Phase1;

        private void InjectDeathCircleController([SiblingsScope] TiledMap tiledMap)
        {
            this.tiledMap = tiledMap;
        }

        private void Awake()
        {
            InjectDependencies("InjectDeathCircleController");
            waitTime = deathCircleValues.WaitTimeInSecond[(int)currentPhase];

            safeCircle = GetAllChildrens()[0].GetComponent<Torus>();
            deathCircle = GetAllChildrens()[1].GetComponent<Torus>();

            transform.position = new Vector3(tiledMap.MapWidthInPixels / 2f, 110, -tiledMap.MapHeightInPixels / 2f);
            float mapDiagonalRadius = Mathf.Sqrt((tiledMap.MapWidthInPixels * tiledMap.MapWidthInPixels) + (tiledMap.MapHeightInPixels * tiledMap.MapHeightInPixels)) / 2;

            CreateCircle(ref safeCircle, mapDiagonalRadius);
            CreateCircle(ref deathCircle, mapDiagonalRadius);
        }

        void Update()
        {
            waitTime -= Time.deltaTime;

            if (waitTime <= 0.0f)
            {
                Debug.Log("WaitTimeFinish");
                if (!hasMoveTimeSet)
                {
                    moveTime = deathCircleValues.MoveTimeInSecond[(int) currentPhase];
                    CreateCircle(ref safeCircle, safeCircle.Radius * deathCircleValues.Shrink[(int)currentPhase]);

                    hasMoveTimeSet = true;
                }

                moveTime -= Time.deltaTime;

                if (moveTime <= 0.0f)
                {
                    Debug.Log("MoveTimeFinish");
                    shrinkTime -= Time.deltaTime;
                    if (shrinkTime <= 0.0f)
                    {
                        Debug.Log("Shink");
                        deathCircle.Radius -= 1f;
                        deathCircle.Create();
                        shrinkTime = InitialShrinkTime;

                        //s'il a fini de shrink, réinitialise + change la phase
                    }
                }
            }
        }

        private void CreateCircle(ref Torus torus, float radius)
        {
            torus.Radius = radius;
            torus.Create();
        }
    }
}

