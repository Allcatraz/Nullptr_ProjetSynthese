namespace ProjetSynthese
{
    public class DeathCircleState : StateMachine
    {

        public override void Execute(ActorAI actor)
        {
            currentAIState = AIState.DeathCircle;
            AIController aiController = actor.ActorController;

            if (aiController.GetAIControllerMode() != AIController.ControllerMode.DeathCircle)
            {
                aiController.SetAIControllerMode(AIController.ControllerMode.DeathCircle);
            }

            aiController.SetDeathCircleFleeDestination(actor);
            if (aiController.MapDestinationIsKnown)
            {
                aiController.AIMoveTarget = AIController.MoveTarget.Map;
                actor.ActorController.Move(actor);
            }
        }
    }
}
