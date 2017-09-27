
namespace ProjetSynthese
{
    public class LootState : StateMachine
    {

        public override void Execute(Actor actor)
        {
            actor.ActorController.Move();
        }
    }
}