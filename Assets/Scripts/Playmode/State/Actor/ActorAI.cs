
using UnityEngine;

namespace ProjetSynthese
{
    public class ActorAI : NetworkGameScript
    {
        public StateMachine CurrentState { get; private set; }
        public AIController ActorController { get; private set; }

        public AIRadar Sensor { get; private set; }
        public AIBrain Brain { get; private set; }
        public EquipmentManager EquipmentManager { get; private set; }

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
        private void Start()
        {
            //Ordre d'initialisation important
            CurrentState = new ExploreState();
            Sensor = new AIRadar();
            ActorController = new AIController(this);
            Brain = new AIBrain(this);
            EquipmentManager = new EquipmentManager(this);
            health.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            health.OnDeath -= OnDeath;
            //drop item
        }

        private void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Execute(this);
            }
            
            AIState nextState = Brain.WhatIsMyNextState(CurrentState.currentAIState);
            if (nextState != CurrentState.currentAIState)
            {
                CurrentState.SwitchState(this, nextState);
            }
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

    }
}