
namespace ProjetSynthese
{
    public class LootState : StateMachine
    {

        public override void Execute(ActorAI actor)
        {
            AIController aiController = (AIController)actor.ActorController;

            if (aiController.GetAIControllerMode() != AIController.ControllerMode.Loot)
            {
                aiController.SetAIControllerMode(AIController.ControllerMode.Loot);
            }

            if (!aiController.ItemTargetDestinationIsKnown)
            {
                aiController.FindTargetItemMapDestination(actor);
            }

            if (aiController.ItemTargetDestinationIsKnown)
            {
                aiController.AIMoveTarget = AIController.MoveTarget.Item;
                actor.ActorController.Move(actor);
                if (aiController.HasReachedItemTargetDestination(actor))
                {
                    aiController.ItemTargetDestinationIsKnown = false;
                    if (actor.Brain.ItemInPerceptionRange != null)
                    {
                        actor.Brain.ItemInPerceptionRange.IsEquipped = true;
                        actor.AIInventory.Add(actor.Brain.ItemInPerceptionRange.gameObject);
                    }
                 }
            }

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Loot);
            if (nextState != AIBrain.AIState.Loot)
            {
                SwitchState(actor, nextState);
            }
            
         }
    }
}