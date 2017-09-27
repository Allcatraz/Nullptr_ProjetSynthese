namespace ProjetSynthese
{
    public class Hunt : StateMachine
    {

        
        public override void Execute(Actor actor)
        {
            actor.ActorController.Move();
        }
    }
}