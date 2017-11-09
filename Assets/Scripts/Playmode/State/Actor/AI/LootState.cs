using UnityEngine;

namespace ProjetSynthese
{
    public class LootState : StateMachine
    {

        public override void Execute(ActorAI actor)
        {
            currentAIState = AIState.Loot;
            AIController aiController = actor.ActorController;

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
                        actor.Brain.ItemInPerceptionRange.gameObject.layer = LayerMask.NameToLayer(AIRadar.LayerNames[(int)AIRadar.LayerType.EquippedItem]);
                        actor.AIInventory.Add(actor.Brain.ItemInPerceptionRange.gameObject);
                        actor.Brain.ItemInPerceptionRange.gameObject.SetActive(false);
                        actor.ServerSetActive(actor.Brain.ItemInPerceptionRange.gameObject, false);
                    }
                 }
            }
         }
    }
}