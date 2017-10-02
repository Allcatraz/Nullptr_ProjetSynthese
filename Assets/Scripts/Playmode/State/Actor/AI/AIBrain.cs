using UnityEngine;

namespace ProjetSynthese
{


    public class AIBrain
    {
        [SerializeField]
        private const float LifeFleeThresholdFactor = 0.20f;

        private readonly float LifeFleeThreshold;
        private float lastLifePointLevel;
        private const float ErrorLifeTolerance = 0.001f;

        public enum AIState { Dead, Explore, Loot, Hunt, Combat, Flee }

        private readonly ActorAI Actor;

        private ActorAI aiInPerceptionRange = null;
        private PlayerController playerInPerceptionRange = null;
        private Item itemInPerceptionRange = null;

        public AIBrain(ActorAI actor)
        {
            this.Actor = actor;
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
                if (Actor.AIHealth.HealthPoints < LifeFleeThreshold)
                {
                    nextState = AIState.Flee;
                }
                else if (ExistShootableOpponent())
                {
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
            if ((lastLifePointLevel - Actor.AIHealth.HealthPoints) > ErrorLifeTolerance)
            {
                return true;
            }
            return false;
        }

        private bool HasBeenHealed()
        {
            if ((lastLifePointLevel - Actor.AIHealth.HealthPoints) < ErrorLifeTolerance)
            {
                return true;
            }
            return false;
        }

        public bool FoundAIInPerceptionRange()
        {

            ActorAI opponentAI = Actor.Sensor.NeareastGameObject<ActorAI>(Actor.transform.position, AIRadar.LayerType.AI);
            if (opponentAI != null)
            {
                aiInPerceptionRange = opponentAI;
                return true;
            }
            return false;
        }

        public bool FoundPlayerInPerceptionRange()
        {
            PlayerController opponentPlayer = Actor.Sensor.NeareastGameObject<PlayerController>(Actor.transform.position, AIRadar.LayerType.Player);
            if (opponentPlayer != null)
            {
                playerInPerceptionRange = opponentPlayer;
                return true;
            }

            return false;
        }

        public bool FoundItemInPerceptionRange()
        {
            Item item = Actor.Sensor.NeareastGameObject<Item>(Actor.transform.position, AIRadar.LayerType.Item);
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
            float sqrtTargetDistance = -1.0f;
            Vector3 directionVector;
            if (playerInPerceptionRange != null)
            {
                directionVector = playerInPerceptionRange.transform.position - Actor.transform.position;
                sqrtTargetDistance = directionVector.sqrMagnitude;
            }
            else if (aiInPerceptionRange != null)
            {
                directionVector = aiInPerceptionRange.transform.position - Actor.transform.position;
                sqrtTargetDistance = directionVector.sqrMagnitude;
            }

            if (sqrtTargetDistance > 0.0f)
            {
                Weapon equippedPrimaryWeapon = (Weapon)Actor.AIInventory.GetPrimaryWeapon().GetItem();
                if (equippedPrimaryWeapon != null)
                {
                    float range = equippedPrimaryWeapon.EffectiveWeaponRange;
                    if (sqrtTargetDistance < range * range)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ExistVisibleOpponent()
        {
            if (FoundPlayerInPerceptionRange() || FoundAIInPerceptionRange())
            {
                if (playerInPerceptionRange != null)
                {
                    if (Actor.Sensor.IsGameObjectHasLineOfSight<PlayerController>(Actor.transform.position, playerInPerceptionRange))
                    {
                        return true;
                    }
                }
                else
                {
                    if (Actor.Sensor.IsGameObjectHasLineOfSight<ActorAI>(Actor.transform.position, aiInPerceptionRange))
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