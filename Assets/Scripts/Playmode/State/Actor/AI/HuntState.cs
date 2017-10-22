using UnityEngine;
namespace ProjetSynthese
{
    public class HuntState : StateMachine
    {

        
        public override void Execute(ActorAI actor)
        {
            //combat pas si pas tiree dessus
            //si vois pas recherche
            //si pas shootable se rapproche
            //pick item selon interet versus interet poursuite
            //interet switch to combat
            AIController aiController = (AIController)actor.ActorController;

            if (aiController.GetAIControllerMode() != AIController.ControllerMode.Hunt)
            {
                aiController.SetAIControllerMode(AIController.ControllerMode.Hunt);
            }

            if (!aiController.OpponentTargetDestinationIsKnown)
            {
                aiController.FindTargetOpponnentMapDestination(actor);
            }

            if (!aiController.ItemTargetDestinationIsKnown)
            {
                aiController.FindTargetItemMapDestination(actor);
            }
            float lootGoalLevel = actor.Brain.goalEvaluator.EvaluateLootGoal();
            float trackGoalLevel = actor.Brain.goalEvaluator.EvaluateLootGoal();
            if ((lootGoalLevel > trackGoalLevel) && aiController.ItemTargetDestinationIsKnown)
            {
                aiController.AIMoveTarget = AIController.MoveTarget.Item;
                actor.ActorController.Move(actor);
                if (aiController.HasReachedItemTargetDestination(actor))
                {
                    aiController.ItemTargetDestinationIsKnown = false;
                    if (actor.Brain.ItemInPerceptionRange != null)
                    {
                        actor.Brain.ItemInPerceptionRange.gameObject.layer = LayerMask.NameToLayer(AIRadar.LayerNames[(int)AIRadar.LayerType.EquippedItem]);
                        actor.AIInventory.Add(actor.Brain.ItemInPerceptionRange.gameObject);
                        actor.Brain.ItemInPerceptionRange.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (aiController.OpponentTargetDestinationIsKnown)
                {
                    aiController.AIMoveTarget = AIController.MoveTarget.Opponent;
                    actor.ActorController.Move(actor);

                    if (aiController.HasReachedOpponentTargetDestination(actor))
                    {
                        aiController.OpponentTargetDestinationIsKnown = false;
                    }
                }
            }

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Hunt);
            if (nextState != AIBrain.AIState.Hunt)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}