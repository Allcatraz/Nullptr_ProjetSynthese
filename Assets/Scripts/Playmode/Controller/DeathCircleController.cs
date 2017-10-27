using Harmony;
using Tiled2Unity;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnPlayerHurtEventHandler(float hurtPoints);

    public delegate void OnDistanceChangeEventHandler(float safeCircleRadius, float deathCircleRadius, float playerRadius);

    //BEN_REVIEW : Ouf...je vais avoir du fun à lire ça...
    
    public class DeathCircleController : GameScript
    {
        //BEN_REVIEW : Pourquoi c'est pas un simple "int" ? Je vois pas l'intérêt d'une enum en fait. Il y a quelque chose
        //             que je comprends pas encore ?
        enum Phases { Phase1, Phase2, Phase3, Phase4, Phase5, Phase6, Phase7, Phase8 }

        [Tooltip("Les informations sur les états du DeathCircle.")]
        [SerializeField] private DeathCircle deathCircleValues;

        //BEN_CORRECTION : Pourquoi ces deux constantes ne sont pas dans deathCircleValues ?
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

            //BEN_CORRECTION : Injection ?
            //                 [Named(R.S.GameObject.SafeCircle)] [ChildrensScope] LineRendererCircle safeCircle
            //                 [Named(R.S.GameObject.DeathCircle)] [ChildrensScope] LineRendererCircle deathCircle
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
                //BEN_REVIEW : Je crois que c'est un travail pour les couroutines ça...mais vous ne saviez peut-être pas
                //             que ça existait dans le temps.
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
            //BEN_CORRECTION : À faire à l'extérieur de cette classe. ;)
            if (isPlayerHit)
            {
                playerHurtTime -= Time.deltaTime;
                if (playerHurtTime <= 0.0f)
                {
                    playerHurtTime = InitialPlayerHurtTime;
                    if (OnPlayerHurt != null) OnPlayerHurt(deathCircleValues.DomagePerSecond[(int) currentPhase]);
                }
            }

            //BEN_REVIEW : Je pense qu'il y a du travail de découpage à faire dans cette classe.
            
            if (OnDistanceChanged != null) OnDistanceChanged(safeCircle.Radius, deathCircle.Radius ,playerRadius);
        }

        //BEN_REVIEW : J'ai cru comprendre que tant que vous ne bougez pas, le player ne prends pas de dégats.
        //             
        //             Autrement dit, il devrait y avoir deux moments où vous vérifiez les dégats pour le joueur durant une frame.
        //             1. Si le joueur a bougé
        //             2. Si le cercle a bougé
        //
        //             Quand le cercle bouge, désabonnez-vous des mouvements du joueur. Quand le cercle ne bouge plus, ré-désabonnez-vous
        //             au mouvements du joueur.              
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

