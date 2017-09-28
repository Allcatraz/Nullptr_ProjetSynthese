
using UnityEngine;

namespace ProjetSynthese
{
    //public class ActorAI : NetworkGameScript, IActorAI
    public class ActorAI : GameScript, IActorAI
    {
        public enum ActorType { None, AI, Vehicle };
        [SerializeField]
        private ActorType actorType = ActorType.None;

        public StateMachine CurrentState { get; private set; }
        public ActorController ActorController { get; private set; }

        public AIRadar Sensor { get; private set; }

        private bool isDead;

        [SerializeField]
        private Inventory inventory;
        
        public Inventory AIInventory { get; private set; }

        [SerializeField]
        private Health health;

        public Health AIHealth { get; private set; }

        private void Start()
        {
            isDead = false;
            switch (actorType)
            {
                case ActorType.None:
                    break;
                case ActorType.AI:
                    CurrentState = new ExploreState();
                    ActorController = new AIController();
                    ((AIController)ActorController).Init();
                    break;
                case ActorType.Vehicle:
                    break;
                default:
                    break;
            }
        }

       
        private void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Execute(this);
            }

            switch (actorType)
            {
                case ActorType.None:
                    break;
                case ActorType.AI:
                    break;
                case ActorType.Vehicle:
                    break;
                default:
                    break;
            }
        }

        public  bool IsDead()
        {
            return isDead;
        }

        public  void SetDead()
        {
            isDead = true;
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