namespace ProjetSynthese
{
    public class HuntState : StateMachine
    {

        
        public override void Execute(ActorAI actor)
        {
            actor.ActorController.Move(actor);

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Explore);
            if (nextState != AIBrain.AIState.Hunt)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}