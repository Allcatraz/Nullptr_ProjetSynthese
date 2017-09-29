namespace ProjetSynthese
{
    public class Hunt : StateMachine
    {

        
        public override void Execute(ActorAI actor)
        {
            actor.ActorController.Move(actor);
        }
    }
}