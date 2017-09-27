namespace ProjetSynthese
{
    public class ExploreState : StateMachine
    {
        public override void Execute(Actor actor)
        {
          
            AIController aiController = (AIController)actor.ActorController;
            if (aiController.MapDestinationIsKnown && aiController.HasReachedMapDestination())
            {
                aiController.MapDestinationIsKnown = false;
            }
            else
            {
                aiController.GenerateRandomDestination();
                aiController.MapDestinationIsKnown = true; 
            }
            aiController.AIMoveTarget = AIController.MoveTarget.Map;
            aiController.AISpeed = AIController.SpeedLevel.Walking;
            actor.ActorController.Move();
        }
    }
}

