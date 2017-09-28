namespace ProjetSynthese
{
    public class ExploreState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
          
            AIController aiController = (AIController)actor.ActorController;
            if (aiController.MapDestinationIsKnown && aiController.HasReachedMapDestination(actor))
            {
                aiController.MapDestinationIsKnown = false;
            }
            else
            {
                aiController.GenerateRandomDestination(actor);
                aiController.MapDestinationIsKnown = true; 
            }
            aiController.AIMoveTarget = AIController.MoveTarget.Map;
            aiController.AISpeed = AIController.SpeedLevel.Walking;
            actor.ActorController.Move(actor);

            if (aiController.AISensor.AIPerceptionLevel != AIRadar.PerceptionLevel.High)
            {
                aiController.AISensor.AIPerceptionLevel = AIRadar.PerceptionLevel.High;
            }
            
        }
    }
}

