
namespace ProjetSynthese
{
    public class LootState : StateMachine
    {

        public override void Execute(ActorAI actor)
        {
            actor.ActorController.Move(actor);

            // Weapon weapon = actor.AISensor.NeareastGameObject<Weapon>(actor.transform.position, AIRadar.LayerType.Item);
            //Item item = actor.AISensor.NeareastGameObject<Item>(actor.transform.position, AIRadar.LayerType.Item);

            //dosomenthing switch state
            //actor.AIInventory.

            AIBrain.AIState nextState = actor.Brain.WhatIsMyNextState(AIBrain.AIState.Explore);
            if (nextState != AIBrain.AIState.Loot)
            {
                SwitchState(actor, nextState);
            }

        }
    }
}