
namespace ProjetSynthese
{
    public class LootState : StateMachine
    {

        public override void Execute(ActorAI actor)
        {
            actor.ActorController.Move();
        }
    }
}