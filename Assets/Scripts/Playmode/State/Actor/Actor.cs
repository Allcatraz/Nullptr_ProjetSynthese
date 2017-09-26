//using System.Collections;
//using System.Collections.Generic;

using NullSurvival;
using UnityEngine;

namespace ProjetSynthese
{
    public class Actor : NetworkGameScript
    {
        public enum ActorType { None, Player, AI, Vehicle };
        [SerializeField]
        private ActorType actorType = ActorType.None;

        public StateMachine CurrentState { get; private set; }
        public ActorController ActorController { get; private set; }



        // Use this for initialization
        private void Start()
        {
            switch (actorType)
            {
                case ActorType.None:
                    break;
                case ActorType.Player:
                    //update ui ....
                    break;
                case ActorType.AI:
                    CurrentState = new LootState();
                    ActorController = new AIController();
                    break;
                case ActorType.Vehicle:
                    break;
                default:
                    break;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            //if (currentState != null)
            //{
            //    currentState.Execute(this);
            //}

            switch (actorType)
            {
                case ActorType.None:
                    break;
                case ActorType.Player:
                    //update ui ....
                    break;
                case ActorType.AI:
                    break;
                case ActorType.Vehicle:
                    break;
                default:
                    break;
            }
        }

        //    public void ChangeState(StateMachine newState)
        //    {

        //#if UNITY_EDITOR
        //        Debug.Assert(currentState != null && newState != null, "État nouveau ou courant de la state machine est null");
        //#endif

        //        currentState = newState;

        //    }
    }
}