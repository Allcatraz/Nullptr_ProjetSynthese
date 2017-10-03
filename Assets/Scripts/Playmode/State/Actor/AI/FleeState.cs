namespace ProjetSynthese
{
    public class FleeState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
            AIController aiController = (AIController)actor.ActorController;

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Explore);
            if (nextState != AIBrain.AIState.Flee)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}