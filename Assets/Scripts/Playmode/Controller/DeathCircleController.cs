using Harmony;
using Tiled2Unity;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnPlayerHurtEventHandler(float hurtPoints);

    public delegate void OnDistanceChangeEventHandler(float safeCircleRadius, float deathCircleRadius, float playerRadius);

    public class DeathCircleController : GameScript
    {
        enum Phases { Phase1, Phase2, Phase3, Phase4, Phase5, Phase6, Phase7, Phase8 }

        [SerializeField] private DeathCircle deathCircleValues;

        private const float InitialShrinkTime = 0.05f;
        private const float InitialPlayerHurtTime = 1.0f;
        private float waitTime;
        private float moveTime;
        private float playerHurtTime;
        private float shrinkTime = InitialShrinkTime;
        private float playerRadius;
        private bool hasMoveTimeSet = false;
        private bool isPlayerHit = false;

        private PlayerMoveEventChannel playerMoveEventChannel;
        private LineRendererCircle safeCircle;
        private LineRendererCircle deathCircle;
        private TiledMap tiledMap;
        private Phases currentPhase = Phases.Phase1;

        public event OnPlayerHurtEventHandler OnPlayerHurt;
        public event OnDistanceChangeEventHandler OnDistanceChanged;

        private void InjectDeathCircleController([SiblingsScope] TiledMap tiledMap,
                                                 [EventChannelScope] PlayerMoveEventChannel playerMoveEventChannel)
        {
            this.tiledMap = tiledMap;
            this.playerMoveEventChannel = playerMoveEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDeathCircleController");
            waitTime = deathCircleValues.WaitTimeInSecond[(int)currentPhase];
            playerHurtTime = InitialPlayerHurtTime;

            safeCircle = GetAllChildrens()[0].GetComponent<LineRendererCircle>();
            deathCircle = GetAllChildrens()[1].GetComponent<LineRendererCircle>();

            playerMoveEventChannel.OnEventPublished += OnPlayerMove;
            
            Vector2 tiledMapScaled = new Vector2((float)tiledMap.MapWidthInPixels / tiledMap.TileWidth, (float)tiledMap.MapHeightInPixels / tiledMap.TileHeight);
            transform.position = new Vector3(tiledMapScaled.x / 2f, 90, -tiledMapScaled.y / 2f);
            float mapDiagonalRadius = Mathf.Sqrt((tiledMapScaled.x * tiledMapScaled.x) + (tiledMapScaled.y * tiledMapScaled.y) / 2);

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
                            deathCircle.Radius -= 0.2f;
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

            // à faire si le joueur est à l'extérieur du cercle
            if (isPlayerHit)
            {
                playerHurtTime -= Time.deltaTime;
                if (playerHurtTime <= 0.0f)
                {
                    playerHurtTime = InitialPlayerHurtTime;
                    if (OnPlayerHurt != null) OnPlayerHurt(deathCircleValues.DomagePerSecond[(int) currentPhase]);
                }
            }

            if (OnDistanceChanged != null) OnDistanceChanged(safeCircle.Radius, deathCircle.Radius ,playerRadius);
        }

        private void OnPlayerMove(PlayerMoveEvent playerMoveEvent)
        {
            float distancePlayerFromSafeCircle = Mathf.Sqrt(Mathf.Pow(deathCircle.transform.position.x - playerMoveEvent.PlayerMover.transform.position.x, 2) +
                                                            Mathf.Pow(deathCircle.transform.position.z - playerMoveEvent.PlayerMover.transform.position.z, 2));
            playerRadius = distancePlayerFromSafeCircle;
            isPlayerHit = distancePlayerFromSafeCircle > deathCircle.Radius;
        }

        private void CreateCircle(ref LineRendererCircle lineRendererCircle, float radius)
        {
            lineRendererCircle.Radius = radius;
            lineRendererCircle.Create();
        }
    }
}

