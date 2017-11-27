using UnityEngine;
using Harmony;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class ActorAI : NetworkGameScript, IInventory, ISwim
    {
        public StateMachine CurrentState { get; private set; }
        public AIController ActorController { get; private set; }
        private bool isSwimming = false;
        private InteractableSensor interactableSensor;

        public AIRadar Sensor { get; private set; }
        public AIBrain Brain { get; private set; }
        public EquipmentManager EquipmentManager { get; private set; }
        public HealthManager HealthManager { get; private set; }

        [Tooltip("Objet représentant l'inventaire de l'AI")]
        [SerializeField]
        private Inventory inventory;

        public Inventory AIInventory
        {
            get
            {
                return inventory;
            }
        }
        [Tooltip("Objet représentant la vie de l'AI")]
        [SerializeField]
        private Health health;

        public Health AIHealth
        {
            get
            {
                return health;
            }
        }

        DeathCircleStatusUpdateEventChannel deathCircleStatusUpdateEventChannel;
        DeathCircleTimeLeftEventChannel deathCircleTimeLeftEventChannel;
        private void InjectDeathCircleController([EventChannelScope] DeathCircleStatusUpdateEventChannel deathCircleStatusUpdateEventChannel, [EventChannelScope] DeathCircleTimeLeftEventChannel deathCircleTimeLeftEventChannel, [EntityScope] InteractableSensor interactableSensor)
        {
            this.deathCircleStatusUpdateEventChannel = deathCircleStatusUpdateEventChannel;
            this.deathCircleTimeLeftEventChannel = deathCircleTimeLeftEventChannel;
            this.interactableSensor = interactableSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectDeathCircleController");
            deathCircleStatusUpdateEventChannel.OnEventPublished += OnDeathCircleFixedUpdate;
            deathCircleTimeLeftEventChannel.OnEventPublished += OnDeathCircleTimeLeftEvent;
        }
        private void Start()
        {
            //Ordre d'initialisation important
            CurrentState = new ExploreState();
            Sensor = new AIRadar();
            ActorController = new AIController(this);
            Brain = new AIBrain(this);
            EquipmentManager = new EquipmentManager(this);
            HealthManager = new HealthManager(this);
            health.OnDeath += OnDeath;

        }



        private void OnDestroy()
        {
            health.OnDeath -= OnDeath;
            deathCircleStatusUpdateEventChannel.OnEventPublished -= OnDeathCircleFixedUpdate;
            deathCircleTimeLeftEventChannel.OnEventPublished -= OnDeathCircleTimeLeftEvent;
        }

        //BEN_REVIEW : Je pense que ce serait suffisant de le faire au "FixedUpdate". Cela pourrait améliorer les performances de votre jeu, car
        //             il y a beaucoup d'appels au moteur de Physique (SphereCastAll par exemple) causés indirectement par un appel à la
        //             fonction "WhatIsMyNextState".
        private void Update()
        {
            //Ordre exécution important
            if (CurrentState != null)
            {
                CurrentState.Execute(this);
            }

            AIState nextState = Brain.WhatIsMyNextState(CurrentState.currentAIState);
            if (nextState != CurrentState.currentAIState)
            {
                CurrentState.SwitchState(this, nextState);
            }

            HealthManager.TendHealth();
        }
        private void OnDeath()
        {
            Destroy(gameObject);
        }

        public void ChangeState(StateMachine newState)
        {

#if UNITY_EDITOR
            Debug.Assert(CurrentState != null && newState != null, "État nouveau ou courant de la state machine est null");
#endif

            CurrentState = newState;

        }

        public Item[] GetProtections()
        {
            ObjectContainedInventory helmet = inventory.GetHelmet();
            ObjectContainedInventory vest = inventory.GetVest();
            return new[] { helmet == null ? null : helmet.GetItem(), vest == null ? null : vest.GetItem() };
        }

        public Weapon GetWeapon()
        {
            ObjectContainedInventory weapon = inventory.GetPrimaryWeapon();
            return weapon == null ? null : weapon.GetItem() as Weapon;
        }

        public void CreateBullet(Vector3 spawnPointPosition, Quaternion spawnPointRotation, Vector3 chamberPosition, float bulletSpeed, float livingTime, int dommage)
        {
            CmdSpawnBullet(spawnPointPosition, spawnPointRotation, chamberPosition, bulletSpeed, livingTime, dommage, GetComponent<NetworkIdentity>());
        }

        public bool IsSwimming
        {
            get
            {
                return isSwimming;
            }
            set
            {
                isSwimming = value;
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                if (isSwimming)
                {
                    ActorController.AISpeed = AIController.SpeedLevel.Swimming;
                    rigidbody.useGravity = false;
                }
                else
                {
                    ActorController.SetAIControllerMode(ActorController.GetAIControllerMode());
                    rigidbody.useGravity = true;
                }
            }
        }

        public void ServerSetActive(GameObject item, bool isActive)
        {
            CmdSetActive(item, isActive);
        }

        private void OnDeathCircleFixedUpdate(DeathCircleStatusUpdateEvent deathCircleStatusUpdateEvent)
        {
            Brain.UpdateDeathCircleKnowledge(deathCircleStatusUpdateEvent.DeathCircleController);
            Vector2 aiPosition = Vector2.zero;
            Vector2 deathCirclePosition = Vector2.zero;
            aiPosition.x = this.transform.position.x;
            aiPosition.y = this.transform.position.z;
            deathCirclePosition.x = Brain.DeathCircleCenterPosition.x;
            deathCirclePosition.y = Brain.DeathCircleCenterPosition.z;
            if (Vector2.Distance(aiPosition, deathCirclePosition) > Brain.DeathCircleRadius)
            {
                health.Hit(Brain.CurrentDeathCircleHurtPoints * Time.deltaTime, true);
                Brain.InjuredByDeathCircle = true;
            }
        }

        private void OnDeathCircleTimeLeftEvent(DeathCircleTimeLeftEvent deathCircleTimeLeftEvent)
        {
            if (deathCircleTimeLeftEvent.Minutes < 1 && deathCircleTimeLeftEvent.Seconds < 30)
            {
                Brain.DeathCircleIsClosing = true;
            }
            else
            {
                Brain.DeathCircleIsClosing = false;
            }
        }

        private void OnInteract()
        {
            GameObject obj = interactableSensor.GetNearestInteractible();

            if (obj != null)
            {
                if (obj.GetComponent<OpenDoor>())
                {
                    obj.GetComponent<OpenDoor>().Use();
                }
            }
        }
    }
}