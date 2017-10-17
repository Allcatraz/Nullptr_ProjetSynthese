namespace ProjetSynthese
{
    public class CombatState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
            AIController aiController = (AIController)actor.ActorController;

            if (aiController.GetAIControllerMode() != AIController.ControllerMode.Combat)
            {
                aiController.SetAIControllerMode(AIController.ControllerMode.Combat);
            }

            //if (!aiController.ItemTargetDestinationIsKnown)
            //{
            //    aiController.FindTargetItemMapDestination(actor);
            //}

            if (aiController.OpponentTargetDestinationIsKnown)
            {
            //    aiController.AIMoveTarget = AIController.MoveTarget.Item;
            //    actor.ActorController.Move(actor);
            //    if (aiController.HasReachedItemTargetDestination(actor))
            //    {
            //        aiController.ItemTargetDestinationIsKnown = false;
            //        if (actor.Brain.ItemInPerceptionRange != null)
            //        {
            //            actor.Brain.ItemInPerceptionRange.gameObject.layer = LayerMask.NameToLayer(AIRadar.LayerNames[(int)AIRadar.LayerType.EquippedItem]);
            //            actor.AIInventory.Add(actor.Brain.ItemInPerceptionRange.gameObject);
            //        }
            //    }
            }

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Combat);
            if (nextState != AIBrain.AIState.Combat)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}