namespace ProjetSynthese
{
    public class CombatState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
           
            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Explore);
            if (nextState != AIBrain.AIState.Combat)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}