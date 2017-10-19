namespace ProjetSynthese
{
    public class HuntState : StateMachine
    {

        
        public override void Execute(ActorAI actor)
        {

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Hunt);
            if (nextState != AIBrain.AIState.Hunt)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}