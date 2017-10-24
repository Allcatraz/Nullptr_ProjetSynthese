namespace ProjetSynthese
{
    public class FleeState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
            AIController aiController = actor.ActorController;

            if (aiController.GetAIControllerMode() != AIController.ControllerMode.Flee)
            {
                aiController.SetAIControllerMode(AIController.ControllerMode.Flee);
            }

            aiController.SetFleeDestination(actor);
            if (aiController.MapDestinationIsKnown)
            {
                aiController.AIMoveTarget = AIController.MoveTarget.Map;
                actor.ActorController.Move(actor);
            }
   
            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Flee);
            if (nextState != AIBrain.AIState.Flee)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}