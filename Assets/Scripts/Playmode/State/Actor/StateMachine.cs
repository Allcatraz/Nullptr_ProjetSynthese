

namespace ProjetSynthese
{
    public abstract class StateMachine
    {
       public abstract void Execute(ActorAI actor);
        public void SwitchState(ActorAI actor,AIBrain.AIState newState)
        {
            switch (newState)
            {
                case AIBrain.AIState.Dead:
                    actor.ChangeState(new DeadState());
                    break;
                case AIBrain.AIState.Explore:
                    actor.ChangeState(new ExploreState());
                    break;
                case AIBrain.AIState.Loot:
                    actor.ChangeState(new LootState());
                    break;
                case AIBrain.AIState.Hunt:
                    actor.ChangeState(new HuntState());
                    break;
                case AIBrain.AIState.Combat:
                    actor.ChangeState(new CombatState());
                    break;
                case AIBrain.AIState.Flee:
                    actor.ChangeState(new FleeState());
                    break;
                default:
                    break;
            }
        }
    }
}