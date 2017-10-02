using UnityEngine;

namespace ProjetSynthese
{


    public class AIBrain
    {
        [SerializeField]
        private const float LifeFleeThresholdFactor = 0.20f;

        private readonly float LifeFleeThreshold;
        private float lastLifePointLevel;
        private const float errorLifeTolerance = 0.001f;

        public enum AIState { Dead, Explore, Loot, Hunt, Combat, Flee }

        private readonly ActorAI actor;

        private ActorAI aiInPerceptionRange = null;
        private PlayerController playerInPerceptionRange = null;
        private Item itemInPerceptionRange = null;

        public AIBrain(ActorAI actor)
        {
            this.actor = actor;
            LifeFleeThreshold = actor.AIHealth.MaxHealthPoints * LifeFleeThresholdFactor;
            lastLifePointLevel = actor.AIHealth.MaxHealthPoints;
        }



        public AIState WhatIsMyNextState(AIState currentState)
        {
            AIState nextState = currentState;
            ResetActualPerception();

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
            AIState nextState = AIState.Explore;

            if (HasBeenInjured())
            {
                if (actor.AIHealth.HealthPoints < LifeFleeThreshold)
                {
                    nextState = AIState.Flee;
                }
                else if (ExistShootableOpponent())
                {
                   //si ennemy in weapon range et visible combat
                    nextState = AIState.Combat;
                }
                else
                {
                    nextState = AIState.Hunt;
                }
            }
            else
            {
                if (ExistVisibleOpponent())
                {
                    nextState = AIState.Hunt;
                }
                else if (FoundItemInPerceptionRange())
                {
                    nextState = AIState.Loot;
                }
            }

            // Weapon weapon = actor.AISensor.NeareastGameObject<Weapon>(actor.transform.position, AIRadar.LayerType.Item);
            //Item item = actor.AISensor.NeareastGameObject<Item>(actor.transform.position, AIRadar.LayerType.Item);

            //dosomenthing switch state
            //actor.AIInventory.


            return nextState;
        }

        private AIState ChooseANewStateFromHuntState()
        {
            AIState nextState = AIState.Hunt;

            return nextState;
        }

        private AIState ChooseANewStateFromLootState()
        {
            AIState nextState = AIState.Loot;

            return nextState;
        }

        private AIState ChooseANewStateFromCombatState()
        {
            AIState nextState = AIState.Dead;

            return nextState;
        }

        private AIState ChooseANewStateFromFleeState()
        {
            AIState nextState = AIState.Dead;

            return nextState;
        }

        private bool HasBeenInjured()
        {
            if ((lastLifePointLevel - actor.AIHealth.HealthPoints) > errorLifeTolerance)
            {
                return true;
            }
            return false;
        }

        private bool HasBeenHealed()
        {
            if ((lastLifePointLevel - actor.AIHealth.HealthPoints) < errorLifeTolerance)
            {
                return true;
            }
            return false;
        }

        public bool FoundAIInPerceptionRange()
        {

            ActorAI opponentAI = actor.Sensor.NeareastGameObject<ActorAI>(actor.transform.position, AIRadar.LayerType.AI);
            if (opponentAI != null)
            {
                aiInPerceptionRange = opponentAI;
                return true;
            }
            return false;
        }

        public bool FoundPlayerInPerceptionRange()
        {
            PlayerController opponentPlayer = actor.Sensor.NeareastGameObject<PlayerController>(actor.transform.position, AIRadar.LayerType.Player);
            if (opponentPlayer != null)
            {
                playerInPerceptionRange = opponentPlayer;
                return true;
            }

            return false;
        }

        public bool FoundItemInPerceptionRange()
        {
            Item item = actor.Sensor.NeareastGameObject<Item>(actor.transform.position, AIRadar.LayerType.Item);
            if (item != null)
            {
                itemInPerceptionRange = item;
                return true;
            }

            return false;
        }

        private bool ExistShootableOpponent()
        {
            if (ExistVisibleOpponent())
            {
                if (IsOpponentInWeaponRange())
                {
                    return true;
                }
                
            }
           
            return false;
        }
        private bool IsOpponentInWeaponRange()
        {
            float sqrtTargetDistance = 0.0f;
            Vector3 directionVector;
            if (playerInPerceptionRange != null)
            {
                directionVector = playerInPerceptionRange.transform.position - actor.transform.position;
                sqrtTargetDistance = directionVector.sqrMagnitude;
                //actor.AIInventory.GetPrimaryWeapon
                
                return true;
            }
            else if (aiInPerceptionRange != null)
            {
                directionVector = aiInPerceptionRange.transform.position - actor.transform.position;
                sqrtTargetDistance = directionVector.sqrMagnitude;
                //actor.AIInventory.GetPrimaryWeapon
                return true;
            } 
           return false;
        }

        private bool ExistVisibleOpponent()
        {
            if (FoundPlayerInPerceptionRange() || FoundAIInPerceptionRange())
            {
                if (playerInPerceptionRange != null)
                {
                    if (actor.Sensor.IsGameObjectHasLineOfSight<PlayerController>(actor.transform.position,playerInPerceptionRange))
                    {
                        return true;
                    }
                }
                else
                {
                    if (actor.Sensor.IsGameObjectHasLineOfSight<ActorAI>(actor.transform.position, aiInPerceptionRange))
                    {
                        return true;
                    }
                }
            }
           
            return false;
        }
        private void ResetActualPerception()
        {
            aiInPerceptionRange = null;
            playerInPerceptionRange = null;
            itemInPerceptionRange = null;
        }

    }
}