using System;
using Harmony;
using Tiled2Unity;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnPlayerHurtEventHandler(float hurtPoints);

    public delegate void OnDistanceChangeEventHandler(float safeCircleRadius, float deathCircleRadius, float playerRadius);

    public delegate void OnDeathCircleStatusUpdateEventHandler();

    public delegate void OnDeathCircleTimeLeftEventHandler(int minutes, int seconds, bool finishWait);

    public class DeathCircleController : GameScript
    {
        public enum Phases
        {
            Phase1,
            Phase2,
            Phase3,
            Phase4,
            Phase5,
            Phase6,
            Phase7,
            Phase8
        }

        [Tooltip("Les informations sur les états du DeathCircle.")]
        [SerializeField]
        private DeathCircle deathCircleValues;

        private const float SpeedDeathCircleShrink = 0.05f;
        private const float SpeedPlayerHurtTime = 1.0f;
        private float timeWait = 0;
        private float timeBeforeMove = 0;
        private float playerHurtTime = SpeedPlayerHurtTime;
        private float timeShrinkDeathCircle = SpeedDeathCircleShrink;
        private float playerRadius = 0;
        private bool isSafeCircleSet = false;
        private bool canPlayerGetHit = false;

        private PlayerMoveEventChannel playerMoveEventChannel;
        private LineRendererCircle safeCircle;
        private LineRendererCircle deathCircle;
        private TiledMap tiledMap;
        private Phases currentPhase = Phases.Phase1;

        public DeathCircle DeathCircleValues { get { return deathCircleValues; } }
        public LineRendererCircle SafeCircle { get { return safeCircle; } }
        public LineRendererCircle DeathCircle { get { return deathCircle; } }
        public Phases CurrentPhase { get { return currentPhase; } }

        public event OnPlayerHurtEventHandler OnPlayerHurt;
        public event OnDistanceChangeEventHandler OnDistanceChanged;
        public event OnDeathCircleStatusUpdateEventHandler OnFixedUpdate;
        public event OnDeathCircleTimeLeftEventHandler OnTimeLeft;

        private void InjectDeathCircleController([SiblingsScope] TiledMap tiledMap,
                                                 [EventChannelScope] PlayerMoveEventChannel playerMoveEventChannel)
        {
            this.tiledMap = tiledMap;
            this.playerMoveEventChannel = playerMoveEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDeathCircleController");
            timeWait = deathCircleValues.WaitTimeInSecond[(int) currentPhase];

            safeCircle = GetAllChildrens()[0].GetComponent<LineRendererCircle>();
            deathCircle = GetAllChildrens()[1].GetComponent<LineRendererCircle>();

            playerMoveEventChannel.OnEventPublished += OnPlayerMove;

            Vector2 tiledMapScaled = new Vector2((float) tiledMap.MapWidthInPixels / tiledMap.TileWidth,
                                                 (float) tiledMap.MapHeightInPixels / tiledMap.TileHeight);
            transform.position = new Vector3(tiledMapScaled.x / 2f, 90, -tiledMapScaled.y / 2f);
            float mapDiagonalRadius = Mathf.Sqrt((tiledMapScaled.x * tiledMapScaled.x) + (tiledMapScaled.y * tiledMapScaled.y) / 2);

            CreateCircle(ref safeCircle, mapDiagonalRadius);
            CreateCircle(ref deathCircle, mapDiagonalRadius);
        }

        private void Update()
        {
            if (currentPhase <= Phases.Phase8)
            {               
                if (!isSafeCircleSet)
                {
                    timeBeforeMove = deathCircleValues.MoveTimeInSecond[(int) currentPhase];
                    CreateCircle(ref safeCircle, safeCircle.Radius * deathCircleValues.Shrink[(int) currentPhase]);
                    isSafeCircleSet = true;
                }
                
                timeWait -= Time.deltaTime;
                if (timeWait <= 0.0f)
                {
                    timeBeforeMove -= Time.deltaTime;
                    if (timeBeforeMove <= 0.0f)
                    {
                        timeShrinkDeathCircle -= Time.deltaTime;
                        if (timeShrinkDeathCircle <= 0.0f)
                        {
                            deathCircle.Radius -= 0.2f;
                            deathCircle.Create();

                            if (OnDistanceChanged != null) OnDistanceChanged(safeCircle.Radius, deathCircle.Radius, playerRadius);

                            if (deathCircle.Radius <= safeCircle.Radius)
                            {
                                if (currentPhase < Phases.Phase8)
                                {
                                    currentPhase += 1;
                                }
                                timeWait = deathCircleValues.WaitTimeInSecond[(int) currentPhase];
                                isSafeCircleSet = false;
                                return;
                            }

                            timeShrinkDeathCircle = SpeedDeathCircleShrink;
                        }
                    }
                }
                CalculateTime();
            }

            // à faire si le joueur est à l'extérieur du cercle
            if (canPlayerGetHit)
            {
                playerHurtTime -= Time.deltaTime;
                if (playerHurtTime <= 0.0f)
                {                    
                    if (OnPlayerHurt != null) OnPlayerHurt(deathCircleValues.DomagePerSecond[(int) currentPhase]);
                    playerHurtTime = SpeedPlayerHurtTime;
                }
            }

            if (OnFixedUpdate != null) OnFixedUpdate();
        }

        private void OnPlayerMove(PlayerMoveEvent playerMoveEvent)
        {
            float distancePlayerFromSafeCircle = Mathf.Sqrt(Mathf.Pow(deathCircle.transform.position.x - playerMoveEvent.PlayerMover.transform.position.x, 2) +
                                                            Mathf.Pow(deathCircle.transform.position.z - playerMoveEvent.PlayerMover.transform.position.z, 2));
            playerRadius = distancePlayerFromSafeCircle;
            canPlayerGetHit = distancePlayerFromSafeCircle > deathCircle.Radius;
        }

        private void CreateCircle(ref LineRendererCircle lineRendererCircle, float radius)
        {
            lineRendererCircle.Radius = radius;
            lineRendererCircle.Create();
        }

        private void CalculateTime()
        {
            bool isWaitFinish = timeWait <= 0;
            timeWait = timeWait <= 0 ? 0 : timeWait;
            int time = (int)(timeWait + timeBeforeMove);
            int minutes = time / 60 >= 0 ? time / 60 : 0;
            int seconds = time % 60 >= 0 ? time % 60 : 0;
            if (OnTimeLeft != null) OnTimeLeft(minutes, seconds, isWaitFinish);
        }
    }
}