using UnityEngine;

namespace ProjetSynthese
{
    public class ExploreState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
          
            AIController aiController = (AIController)actor.ActorController;

            if (aiController.GetAIControllerMode() != AIController.ControllerMode.Explore)
            {
                aiController.SetAIControllerMode(AIController.ControllerMode.Explore);
            }

            if (!aiController.MapDestinationIsKnown)
            {
                aiController.GenerateRandomDestination(actor);
                aiController.MapDestinationIsKnown = true;
            }

            aiController.AIMoveTarget = AIController.MoveTarget.Map;
            actor.ActorController.Move(actor);

            if (aiController.HasReachedMapDestination(actor))
            {
                aiController.MapDestinationIsKnown = false;
            }
            LayerMask layerMask = LayerMask.NameToLayer("Item");
            Weapon weapon = aiController.AISensor.NeareastGameObject<Weapon>(actor.transform.position, layerMask);
            GameObject item = aiController.AISensor.NeareastGameObject<GameObject>(actor.transform.position, layerMask);
            //dosomenthing switch state
            //actor.AIInventory.
            int g = 1;
        }
    }
}

