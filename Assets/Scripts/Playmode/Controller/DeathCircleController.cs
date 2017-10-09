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

        private LineRendererCircle safeCircle;
        private LineRendererCircle deathCircle;
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

            safeCircle = GetAllChildrens()[0].GetComponent<LineRendererCircle>();
            deathCircle = GetAllChildrens()[1].GetComponent<LineRendererCircle>();

            transform.position = new Vector3(tiledMap.MapWidthInPixels / 2f, 90, -tiledMap.MapHeightInPixels / 2f);
            float mapDiagonalRadius = Mathf.Sqrt((tiledMap.MapWidthInPixels * tiledMap.MapWidthInPixels) + (tiledMap.MapHeightInPixels * tiledMap.MapHeightInPixels)) / 2;

            CreateCircle(ref safeCircle, mapDiagonalRadius);
            CreateCircle(ref deathCircle, mapDiagonalRadius);
        }

        void FixedUpdate()
        {
            if (currentPhase <= Phases.Phase8)
            {
                waitTime -= Time.deltaTime;

                if (waitTime <= 0.0f)
                {
                    if (!hasMoveTimeSet)
                    {
                        moveTime = deathCircleValues.MoveTimeInSecond[(int) currentPhase];
                        CreateCircle(ref safeCircle, safeCircle.Radius * deathCircleValues.Shrink[(int) currentPhase]);

                        hasMoveTimeSet = true;
                    }

                    moveTime -= Time.deltaTime;

                    if (moveTime <= 0.0f)
                    {
                        shrinkTime -= Time.deltaTime;
                        if (shrinkTime <= 0.0f)
                        {
                            deathCircle.Radius -= 1f;
                            deathCircle.Create();

                            if (deathCircle.Radius <= safeCircle.Radius)
                            {
                                if (currentPhase < Phases.Phase8)
                                {
                                    currentPhase += 1;
                                }
                                waitTime = deathCircleValues.WaitTimeInSecond[(int) currentPhase];
                                hasMoveTimeSet = false;
                                return;
                            }

                            shrinkTime = InitialShrinkTime;
                        }
                    }
                }
            }
        }

        private void CreateCircle(ref LineRendererCircle lineRendererCircle, float radius)
        {
            lineRendererCircle.Radius = radius;
            lineRendererCircle.Create();
        }
    }
}

