
namespace ProjetSynthese
{
    public class AIDecisionManager
    {
        private readonly AIBrain brain;
        private readonly ActorAI actor;
        public AIDecisionManager(ActorAI actor,AIBrain brain)
        {
            this.brain = brain;
            this.actor = actor;
        }
        public AIState WhatIsMyNextState(AIState currentState)
        {
            AIState nextState = currentState;
            
            switch (currentState)
            {
                case AIState.Dead:
                    nextState = ChooseANewStateFromDeadState();
                    break;
                case AIState.Explore:
                    nextState = ChooseANewStateFromExploreState();
                    break;
                case AIState.Loot:
                    nextState = ChooseANewStateFromLootState();
                    break;
                case AIState.Hunt:
                    nextState = ChooseANewStateFromHuntState();
                    break;
                case AIState.Combat:
                    nextState = ChooseANewStateFromCombatState();
                    break;
                case AIState.Flee:
                    nextState = ChooseANewStateFromFleeState();
                    break;
                case AIState.DeathCircle:
                    nextState = ChooseANewStateFromDeathCircleState();
                    break;
                default:
                    break;
            }

            return nextState;
        }

        private AIState ChooseANewStateFromDeadState()
        {
            AIState nextState = AIState.Dead;
            return nextState;
        }

        private AIState ChooseANewStateFromExploreState()
        {

            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && brain.DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
            if (nextState == AIState.None)
            {
                if (brain.ExistVisibleOpponent())
                {
                    if (!brain.HasPrimaryWeaponEquipped)
                    {
                        if (!brain.FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }
                    }
                    else
                    {
                        nextState = AIState.Hunt;
                    }
                }
                else if (brain.FoundItemInPerceptionRange())
                {
                    nextState = AIState.Loot;
                }
            }

            if (nextState == AIState.None)
            {
                nextState = AIState.Explore;
            }
            return nextState;
        }

        private AIState ChooseANewStateFromHuntState()
        {
            AIState nextState = AIState.None;
            nextState =HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && brain.DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
            if (nextState == AIState.None)
            {
                if (brain.ExistVisibleOpponent())
                {
                    if (!brain.HasPrimaryWeaponEquipped)
                    {
                        if (!brain.FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }
                    }
                    else
                    {
                        if (brain.ExistShootableOpponent())
                        {
                            nextState = AIState.Combat;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }

                    }
                }
                else if (brain.FoundItemInPerceptionRange())
                {
                    nextState = AIState.Loot;
                }
            }
            if (nextState == AIState.None)
            {
                nextState = AIState.Explore;
            }

            return nextState;
        }

        private AIState ChooseANewStateFromLootState()
        {
            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && brain.DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
            if (nextState == AIState.None)
            {
                if (brain.ExistVisibleOpponent())
                {
                    if (!brain.HasPrimaryWeaponEquipped)
                    {
                        if (!brain.FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                    }
                    else
                    {
                        nextState = AIState.Hunt;
                    }
                }
                else
                {
                    brain.UpdateItemLootKnowledge();
                    if (brain.ItemInPerceptionRange != null)
                    {
                        nextState = AIState.Loot;
                    }
                    else if (brain.FoundItemInPerceptionRange())
                    {
                        nextState = AIState.Loot;
                    }
                    else
                    {
                        nextState = AIState.Explore;
                    }
                }
            }
            return nextState;
        }

        private AIState ChooseANewStateFromCombatState()
        {
            AIState nextState = AIState.None;

            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && brain.DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
            if (nextState == AIState.None)
            {
                if (brain.ExistShootableOpponent())
                {
                    nextState = AIState.Combat;
                }
                else if (brain.ExistVisibleOpponent())
                {
                    if (!brain.HasPrimaryWeaponEquipped)
                    {
                        if (!brain.FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }
                    }
                    else
                    {
                        nextState = AIState.Hunt;
                    }
                }
                else if (brain.FoundItemInPerceptionRange())
                {
                    nextState = AIState.Loot;
                }
                else
                {
                    nextState = AIState.Explore;
                }
            }

            return nextState;
        }

        private AIState ChooseANewStateFromFleeState()
        {
            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && brain.DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
            if (nextState == AIState.None)
            {
                if (brain.ExistVisibleOpponent())
                {
                    if (!brain.HasPrimaryWeaponEquipped)
                    {
                        if (!brain.FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }
                    }
                    else
                    {
                        nextState = AIState.Hunt;
                    }
                }
                else if (brain.FoundItemInPerceptionRange())
                {
                    nextState = AIState.Loot;
                }
            }

            if (nextState == AIState.None)
            {
                nextState = AIState.Explore;
            }

            return nextState;
        }

        private AIState ChooseANewStateFromDeathCircleState()
        {
            AIState nextState = AIState.None;
            if (brain.InjuredByDeathCircle)
            {
                nextState = AIState.DeathCircle;
            }
            else if (brain.DeathCircleIsClosing)
            {
                if (brain.CurrentDistanceOutsideSafeCircle > 0.0f)
                {
                    nextState = AIState.DeathCircle;
                }
                else
                {
                    nextState = AIState.Explore;
                }
            }
            else
            {
                nextState = AIState.Explore;
            }
            return nextState;
            //AIState nextState = AIState.None;
            //nextState = HasBeenInjuredRelatedStateCheck();
            //if (nextState == AIState.None && DeathCircleIsClosing)
            //{
            //    nextState = AIState.DeathCircle;
            //}
            //if (nextState == AIState.None)
            //{
            //    if (ExistVisibleOpponent())
            //    {
            //        if (!hasPrimaryWeaponEquipped)
            //        {
            //            if (!FoundItemInPerceptionRange())
            //            {
            //                nextState = AIState.Flee;
            //            }
            //            else
            //            {
            //                nextState = AIState.Hunt;
            //            }
            //        }
            //        else
            //        {
            //            nextState = AIState.Hunt;
            //        }
            //    }
            //    else if (FoundItemInPerceptionRange())
            //    {
            //        nextState = AIState.Loot;
            //    }
            //}

            //if (nextState == AIState.None)
            //{
            //    nextState = AIState.Explore;
            //}

            //return nextState;
        }

        private AIState HasBeenInjuredRelatedStateCheck()
        {
            AIState nextState = AIState.None;
            if (brain.HasBeenInjured() || brain.InjuredByDeathCircle)
            {
                if (actor.AIHealth.HealthPoints < 0.0f)
                {
                    nextState = AIState.Dead;
                }
                else if (brain.InjuredByDeathCircle)
                {
                    brain.InjuredByDeathCircle = false;
                    nextState = AIState.DeathCircle;
                }
                else if (actor.AIHealth.HealthPoints < brain.LifeFleeThreshold)
                {
                    nextState = AIState.Flee;
                }
                else if (brain.ExistShootableOpponent())
                {
                    nextState = AIState.Combat;
                }
                else
                {
                    nextState = AIState.Hunt;
                }
            }

            return nextState;
        }
    }
}