﻿using UnityEngine;

namespace ProjetSynthese
{


    public class AIBrain
    {
        [SerializeField]
        private const float LifeFleeThresholdFactor = 0.20f;

        private readonly float LifeFleeThreshold;
        private float lastLifePointLevel;
        private const float ErrorLifeTolerance = 0.001f;

        public enum AIState { None, Dead, Explore, Loot, Hunt, Combat, Flee }

        private readonly ActorAI Actor;

        private ActorAI aiInPerceptionRange = null;
        private PlayerController playerInPerceptionRange = null;
        private Item itemInPerceptionRange = null;

        public Item ItemInPerceptionRange
        {
            get
            { return itemInPerceptionRange; }
            private set
            { itemInPerceptionRange = value; }
        }

        public AIBrain(ActorAI actor)
        {
            this.Actor = actor;
            LifeFleeThreshold = actor.AIHealth.MaxHealthPoints * LifeFleeThresholdFactor;
            lastLifePointLevel = actor.AIHealth.MaxHealthPoints;
            ResetActualPerception();
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
            if (nextState == AIState.None)
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

            if (nextState == AIState.None)
            {
                nextState = AIState.Explore;
            }

            return nextState;
        }

        private AIState ChooseANewStateFromHuntState()
        {
            AIState nextState = AIState.Explore;

            return nextState;
        }

        private AIState ChooseANewStateFromLootState()
        {
            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None)
            {
                if (ExistVisibleOpponent())
                {
                    nextState = AIState.Hunt;
                }
                else
                {
                    UpdateItemLootKnowledge();
                    if (itemInPerceptionRange != null)
                    {
                        nextState = AIState.Loot;
                    }
                    else if (FoundItemInPerceptionRange())
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
            AIState nextState = AIState.Explore;
            return nextState;
        }

        private AIState ChooseANewStateFromFleeState()
        {
            AIState nextState = AIState.Explore;

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

            //ActorAI opponentAI = Actor.Sensor.NeareastGameObject<ActorAI>(Actor.transform.position, AIRadar.LayerType.AI);
            //if (opponentAI != null)
            //{
            //    aiInPerceptionRange = opponentAI;
            //    return true;
            //}
            return false;
        }

        public bool FoundPlayerInPerceptionRange()
        {
            //PlayerController opponentPlayer = Actor.Sensor.NeareastGameObject<PlayerController>(Actor.transform.position, AIRadar.LayerType.Player);
            //if (opponentPlayer != null)
            //{
            //    playerInPerceptionRange = opponentPlayer;
            //    return true;
            //}

            return false;
        }

        public bool FoundItemInPerceptionRange()
        {
            Item item = Actor.Sensor.NeareastNonEquippedItem(Actor.transform.position);
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
                    if (Actor.Sensor.IsGameObjectHasLineOfSight(Actor.transform.position, playerInPerceptionRange))
                    {
                        return true;
                    }
                }
                else
                {
                    if (Actor.Sensor.IsGameObjectHasLineOfSight(Actor.transform.position, aiInPerceptionRange))
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

        public void UpdateItemOnMapKnowledge(Item newTargetItem)
        {
            itemInPerceptionRange = newTargetItem;
        }

        private void UpdateItemLootKnowledge()
        {
            if (itemInPerceptionRange != null)
            {
                if (itemInPerceptionRange.gameObject.layer == LayerMask.NameToLayer(AIRadar.LayerNames[(int)AIRadar.LayerType.EquippedItem]))
                {
                    itemInPerceptionRange = null;
                    ((AIController)Actor.ActorController).ItemTargetDestinationIsKnown = false;
                }
            }
            else
            {
                ((AIController)Actor.ActorController).ItemTargetDestinationIsKnown = false;
            }
        }


        private AIState HasBeenInjuredRelatedStateCheck()
        {
            AIState nextState = AIState.None;
            if (HasBeenInjured())
            {
                Weapon equippedPrimaryWeapon = (Weapon)Actor.AIInventory.GetPrimaryWeapon().GetItem();
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
            return nextState;
        }

    }
}