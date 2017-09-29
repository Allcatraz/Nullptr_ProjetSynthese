namespace ProjetSynthese
{

    public enum AIState { Dead, Explore, Loot, Hunt, Combat, Flee }

    public class AIBrain
    {
        private readonly ActorAI actor;

        public AIBrain(ActorAI actor)
        {
            this.actor = actor;
        }

        public AIState WhatIsMyNextState(AIState currentState)
        {
            AIState nextState = currentState;

            switch (currentState)
            {
                case AIState.Dead:
                    nextState = ChooseANewStateFromDeadState();
                    break;
                case AIState.Explore:
                    nextState = ChooseANewStateFromExploreState();
                    break;
                case AIState.Loot:
                    nextState = ChooseANewStateFromLootState();
                    break;
                case AIState.Hunt:
                    nextState = ChooseANewStateFromHuntState();
                    break;
                case AIState.Combat:
                    nextState = ChooseANewStateFromCombatState();
                    break;
                case AIState.Flee:
                    nextState = ChooseANewStateFromFleeState();
                    break;
                default:
                    break;
            }

            return nextState;
        }

        private AIState ChooseANewStateFromDeadState()
        {
            AIState nextState = AIState.Dead;

            return nextState;
        }

        private AIState ChooseANewStateFromExploreState()
        {
            AIState nextState = AIState.Explore;
            //Si shot on
            //Si < 25% life flee
            //else , si ennemy in weapon range et visible combat
            //       else hunt
            //Si ennemy in range visible hunt
            //Si item in range loot


            // Weapon weapon = actor.AISensor.NeareastGameObject<Weapon>(actor.transform.position, AIRadar.LayerType.Item);
            //Item item = actor.AISensor.NeareastGameObject<Item>(actor.transform.position, AIRadar.LayerType.Item);

            //dosomenthing switch state
            //actor.AIInventory.
           

            return nextState;
        }

        private AIState ChooseANewStateFromHuntState()
        {
            AIState nextState = AIState.Hunt;

            return nextState;
        }

        private AIState ChooseANewStateFromLootState()
        {
            AIState nextState = AIState.Loot;

            return nextState;
        }

        private AIState ChooseANewStateFromCombatState()
        {
            AIState nextState = AIState.Dead;

            return nextState;
        }

        private AIState ChooseANewStateFromFleeState()
        {
            AIState nextState = AIState.Dead;

            return nextState;
        }
    }
}