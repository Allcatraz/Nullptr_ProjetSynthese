namespace ProjetSynthese
{
    public class HuntState : StateMachine
    {

        
        public override void Execute(ActorAI actor)
        {

            //AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Explore);
            //if (nextState != AIBrain.AIState.Hunt)
            //{
            //    SwitchState(actor, nextState);
            //}
            //doit quitte ce state si pas d'arme équipé et n'en trouve pas à porté
            SwitchState(actor, AIBrain.AIState.Combat);
        }
    }
}