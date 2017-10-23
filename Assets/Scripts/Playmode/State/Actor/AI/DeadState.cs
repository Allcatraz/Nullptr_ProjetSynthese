
namespace ProjetSynthese
{

    public class DeadState : StateMachine
    {
        public override void Execute(ActorAI actor)
        {
            if (actor != null)
            {
                actor.SetDead();
            }
         }
    }
}