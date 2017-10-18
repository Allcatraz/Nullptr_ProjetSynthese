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

            if (!aiController.OpponentTargetDestinationIsKnown)
            {
                aiController.FindTargetOpponnentMapDestination(actor);
            }

            if (aiController.OpponentTargetDestinationIsKnown)
            {
                aiController.AIMoveTarget = AIController.MoveTarget.Opponent;
                actor.ActorController.Move(actor);
                //faudrait modifier la reached selon weapon range
                if (aiController.HasReachedOpponentTargetDestination(actor))
                {
                    aiController.OpponentTargetDestinationIsKnown = false;
                }
            }
            //shot icitt et revérifie line of sight ou pas
            //reload ammunition 
            //change weapon
            switch (actor.Brain.CurrentOpponentType)
            {
                case AIBrain.OpponentType.None:
                    break;
                case AIBrain.OpponentType.AI:
                    break;
                case AIBrain.OpponentType.Player:
                    break;
                default:
                    break;
            }

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Combat);
            if (nextState != AIBrain.AIState.Combat)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}