

namespace ProjetSynthese
{
    public abstract class StateMachine
    {
        public AIState currentAIState { get; set; }
        public abstract void Execute(ActorAI actor);
        public void SwitchState(ActorAI actor,AIState newState)
        {
            switch (newState)
            {
                case AIState.Dead:
                    actor.ChangeState(new DeadState());
                    break;
                case AIState.Explore:
                    actor.ChangeState(new ExploreState());
                    break;
                case AIState.Loot:
                    actor.ChangeState(new LootState());
                    break;
                case AIState.Hunt:
                    actor.ChangeState(new HuntState());
                    break;
                case AIState.Combat:
                    actor.ChangeState(new CombatState());
                    break;
                case AIState.Flee:
                    actor.ChangeState(new FleeState());
                    break;
                case AIState.DeathCircle:
                    actor.ChangeState(new DeathCircleState());
                    break;
                default:
                    break;
            }
        }
    }
}