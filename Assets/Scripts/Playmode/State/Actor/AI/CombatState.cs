namespace ProjetSynthese
{
    public class CombatState : StateMachine
    {
        public override void Execute(Actor actor)
        {
            actor.ActorController.Move();
        }
    }
}