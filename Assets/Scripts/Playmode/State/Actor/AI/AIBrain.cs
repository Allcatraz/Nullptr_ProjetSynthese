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

        private bool hasPrimaryWeaponEquipped = false;

        public enum AIState { None, Dead, Explore, Loot, Hunt, Combat, Flee }

        private readonly ActorAI Actor;

        public enum OpponentType { None, AI, Player };
        private OpponentType currentOpponentType;
        public OpponentType CurrentOpponentType
        {
            get
            { return currentOpponentType; }
            set
            { currentOpponentType = value; }
        }

        private ActorAI aiInPerceptionRange = null;
        private PlayerController playerInPerceptionRange = null;
        private Item itemInPerceptionRange = null;

        public ActorAI AiInPerceptionRange
        {
            get
            { return aiInPerceptionRange; }
            private set
            { aiInPerceptionRange = value; }
        }

        public PlayerController PlayerInPerceptionRange
        {
            get
            { return playerInPerceptionRange; }
            private set
            { playerInPerceptionRange = value; }
        }

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
            currentOpponentType = OpponentType.None;
        }



        public AIState WhatIsMyNextState(AIState currentState)
        {
            AIState nextState = currentState;
            UpdateWeaponKnowledge();
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
            //droper items premire passe
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
                    if (!hasPrimaryWeaponEquipped)
                    {
                        if (!FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }
                    }
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
            AIState nextState = AIState.Combat;

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
                    if (!hasPrimaryWeaponEquipped)
                    {
                        if (!FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }
                    }
                    
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
            AIState nextState = AIState.None;

            nextState = HasBeenInjuredRelatedStateCheck();

            if (nextState == AIState.None)
            {
                if (ExistShootableOpponent())
                {
                    nextState = AIState.Combat;
                }
                else if (ExistVisibleOpponent())
                {
                    if (!hasPrimaryWeaponEquipped)
                    {
                        if (!FoundItemInPerceptionRange())
                        {
                            nextState = AIState.Flee;
                        }
                        else
                        {
                            nextState = AIState.Hunt;
                        }
                    }
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

            ActorAI opponentAI = Actor.Sensor.NeareastNonAllyAI(Actor);
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
            Item item = Actor.Sensor.NeareastNonEquippedItem(Actor.transform.position);
            if (item != null)
            {
                itemInPerceptionRange = item;
                return true;
            }
            return false;
        }

        public bool ExistShootableOpponent()
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
        public bool IsOpponentInWeaponRange()
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

            if (sqrtTargetDistance > 0.0f && hasPrimaryWeaponEquipped)
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
            //if (FoundPlayerInPerceptionRange() || FoundAIInPerceptionRange())
            //{
            //    if (playerInPerceptionRange != null)
            //    {
            //        if (Actor.Sensor.IsGameObjectHasLineOfSight(Actor.transform.position, playerInPerceptionRange))
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        if (Actor.Sensor.IsGameObjectHasLineOfSight(Actor.transform.position, aiInPerceptionRange))
            //        {
            //            return true;
            //        }
            //    }
            //}

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

        public void UpdateOpponentOnMapKnowledge(PlayerController opponentPlayer, ActorAI opponnentAI)
        {
            playerInPerceptionRange = opponentPlayer;
            aiInPerceptionRange = opponnentAI;
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

        public void UpdateWeaponKnowledge()
        {
            Cell cell = Actor.AIInventory.GetPrimaryWeapon();
            Weapon equippedPrimaryWeapon = null;
            if (cell != null)
            {
                equippedPrimaryWeapon = (Weapon)cell.GetItem();
            }
            
            if (equippedPrimaryWeapon != null)
            {
                hasPrimaryWeaponEquipped = true;
            }
            else
            {
                hasPrimaryWeaponEquipped = false;
            }
            //choix weapon (quand pas possible desequuiped)
            //reload
            //ammunition
            //si pas de munition ou plus de munitions: unequipped weapon
            //essai de reloader
            //essai equiper nouvelle weapon
        }

    }
}