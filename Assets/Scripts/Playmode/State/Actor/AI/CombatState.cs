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
                
                if (aiController.HasReachedOpponentTargetDestination(actor))
                {
                    aiController.OpponentTargetDestinationIsKnown = false;
                }
            }
            if (actor.Brain.ExistShootableOpponent())
            {
                aiController.Shoot(actor.Brain.CurrentOpponentType);
            }
            //faut un call en quelque part pour weapon knoledge update
            //avant décision possiblement dans brain et apres shooting
            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Combat);
            if (nextState != AIBrain.AIState.Combat)
            {
                SwitchState(actor, nextState);
            }
        }
    }
}