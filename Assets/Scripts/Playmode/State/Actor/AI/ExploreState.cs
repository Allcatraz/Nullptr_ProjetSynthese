using UnityEngine;

namespace ProjetSynthese
{
    public class ExploreState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
            currentAIState = AIState.Explore;
            AIController aiController = actor.ActorController;

            if (aiController.GetAIControllerMode() != AIController.ControllerMode.Explore)
            {
                aiController.SetAIControllerMode(AIController.ControllerMode.Explore);
            }

            if (!aiController.MapDestinationIsKnown)
            {
                aiController.GenerateRandomDestination(actor);
            }

            aiController.AIMoveTarget = AIController.MoveTarget.Map;
            actor.ActorController.Move(actor);

            if (aiController.HasReachedMapDestination(actor))
            {
                aiController.MapDestinationIsKnown = false;
            }
        }
    }
}

