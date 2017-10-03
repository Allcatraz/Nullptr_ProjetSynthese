
namespace ProjetSynthese
{
    public class LootState : StateMachine
    {

        public override void Execute(ActorAI actor)
        {
            actor.ActorController.Move(actor);

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Explore);
            if (nextState != AIBrain.AIState.Loot)
            {
                SwitchState(actor, nextState);
            }

        }
    }
}