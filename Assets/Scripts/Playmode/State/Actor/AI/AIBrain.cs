using UnityEngine;

namespace ProjetSynthese
{


    public class AIBrain
    {
        #region Parameters
        private const float ErrorCirclesGapTolerance = 0.001f;
        private const float LifeFleeThresholdFactor = 0.20f;
        private readonly float LifeFleeThreshold;
        private float lastLifePointLevel;
        private const float ErrorLifeTolerance = 0.001f;
        private const float MaxUsefulStoredHealItem = 5.0f;
        private const float MaxUsefulStoredBoostItem = 5.0f;
        private const float MaxUsefulStoredAmmoPackItem = 5.0f;
        private const int NumberOfAmmoPackType = 4;
        public readonly float HealEfficiencyMaximum = 100.0f;
        public readonly float BoostEfficiencyMaximum = 35.0f;
        public readonly float BagCapacityMaximum = 300.0f;
        public readonly float HelmetProtectionMaximum = 50.0f;
        public readonly float VestProtectionMaximum = 50.0f;

        private readonly float WeaponEffectiveRangeDamping = 0.1f;

        private readonly ActorAI Actor;
        public readonly GoalEvaluator goalEvaluator;

        public enum OpponentType { None, AI, Player };
        #endregion

        #region Knowledge

        public Vector3 SafeCircleCenterPosition { get; private set; }
        public Vector3 DeathCircleCenterPosition { get; private set; }
        public float SafeCircleRadius { get; private set; }
        public float DeathCircleRadius { get; private set; }
        public float CurrentDeathCircleHurtPoints { get; private set; }
        public float CurrentDistanceOutsideSafeCircle { get; private set; }

        public bool DeathCircleIsClosing { get; set; }
        public bool InjuredByDeathCircle { get; set; }
        private float healthRatio = 1.0f;
        private float healNumberStorageRatio = 0.0f;
        private float boostNumberStorageRatio = 0.0f;
        private float[] ammoPackNumberStorageRatio = new float[NumberOfAmmoPackType];
        private float bagCapacityRatio = 0.0f;
        private float helmetProtectionRatio = 0.0f;
        private float vestProtectionRatio = 0.0f;

        private ObjectContainedInventory inventoryBestBag = null;
        private ObjectContainedInventory inventoryBestHeal = null;
        private ObjectContainedInventory inventoryBestBoost = null;
        private ObjectContainedInventory inventoryBestHelmet = null;
        private ObjectContainedInventory inventoryBestVest = null;

        public ObjectContainedInventory InventoryBestBag { get { return inventoryBestBag; } set { inventoryBestBag = value; } }
        public ObjectContainedInventory InventoryBestHeal { get { return inventoryBestHeal; } set { inventoryBestHeal = value; } }
        public ObjectContainedInventory InventoryBestBoost { get { return inventoryBestBoost; } set { inventoryBestBoost = value; } }
        public ObjectContainedInventory InventoryBestHelmet { get { return inventoryBestHelmet; } set { inventoryBestHelmet = value; } }
        public ObjectContainedInventory InventoryBestVest { get { return inventoryBestVest; } set { inventoryBestVest = value; } }



        public float HealthRatio { get { return healthRatio; } private set { healthRatio = value; } }
        public float HealNumberStorageRatio { get { return healNumberStorageRatio; } private set { healNumberStorageRatio = value; } }
        public float BoostNumberStorageRatio { get { return boostNumberStorageRatio; } private set { boostNumberStorageRatio = value; } }
        public float BagCapacityRatio { get { return bagCapacityRatio; } private set { bagCapacityRatio = value; } }
        public float HelmetProtectionRatio { get { return helmetProtectionRatio; } private set { helmetProtectionRatio = value; } }
        public float VestProtectionRatio { get { return vestProtectionRatio; } private set { vestProtectionRatio = value; } }


        private bool hasPrimaryWeaponEquipped = false;
        private bool hasHelmetEquipped = false;
        private bool hasVestEquipped = false;
        private bool hasBagEquipped = false;
        public bool HasHelmetEquipped
        {
            get { return hasHelmetEquipped; }
            set { hasHelmetEquipped = value; }
        }
        public bool HasVestEquipped
        {
            get { return hasVestEquipped; }
            set { hasVestEquipped = value; }
        }
        public bool HasPrimaryWeaponEquipped
        {
            get { return hasPrimaryWeaponEquipped; }
            set { hasPrimaryWeaponEquipped = value; }
        }
        public bool HasBagEquipped
        {
            get { return hasBagEquipped; }
            set { hasBagEquipped = value; }
        }

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
        #endregion

        public AIBrain(ActorAI actor)
        {
            this.Actor = actor;
            this.goalEvaluator = new GoalEvaluator(actor);
            LifeFleeThreshold = actor.AIHealth.MaxHealthPoints * LifeFleeThresholdFactor;
            lastLifePointLevel = actor.AIHealth.MaxHealthPoints;
            ResetActualPerception();
            currentOpponentType = OpponentType.None;
            InjuredByDeathCircle = false;
            DeathCircleIsClosing = false;
        }

        public AIState WhatIsMyNextState(AIState currentState)
        {
            AIState nextState = currentState;
            UpdateInventoryKnowledge();
            UpdateWeaponKnowledge();
            UpdateProtectionKnowledge();
            UpdateSupportKnowledge();
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
            if (nextState == AIState.None && DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
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
                    else
                    {
                        nextState = AIState.Hunt;
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
            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
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
                    else
                    {
                        if (ExistShootableOpponent())
                        {
                            nextState = AIState.Combat;
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

        private AIState ChooseANewStateFromLootState()
        {
            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
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
                    }
                    else
                    {
                        nextState = AIState.Hunt;
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
            if (nextState == AIState.None && DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
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
                    else
                    {
                        nextState = AIState.Hunt;
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
            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
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
                    else
                    {
                        nextState = AIState.Hunt;
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

        private AIState ChooseANewStateFromDeathCircleState()
        {
            AIState nextState = AIState.None;
            nextState = HasBeenInjuredRelatedStateCheck();
            if (nextState == AIState.None && DeathCircleIsClosing)
            {
                nextState = AIState.DeathCircle;
            }
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
                    else
                    {
                        nextState = AIState.Hunt;
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

        private bool HasBeenInjured()
        {
            if ((lastLifePointLevel - Actor.AIHealth.HealthPoints) > ErrorLifeTolerance)
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
                directionVector.y = 0.0f;
                sqrtTargetDistance = directionVector.sqrMagnitude;
            }
            else
            {
                directionVector = aiInPerceptionRange.transform.position - Actor.transform.position;
                directionVector.y = 0.0f;
                sqrtTargetDistance = directionVector.sqrMagnitude;
            }

            if (sqrtTargetDistance > 0.0f && hasPrimaryWeaponEquipped)
            {
                Weapon equippedPrimaryWeapon = (Weapon)Actor.AIInventory.GetPrimaryWeapon().GetItem();
                if (equippedPrimaryWeapon != null)
                {
                    float range = equippedPrimaryWeapon.EffectiveWeaponRange * WeaponEffectiveRangeDamping;
                    if (sqrtTargetDistance < range * range)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ExistVisibleOpponent()
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
                else if(aiInPerceptionRange != null)
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
            if (HasBeenInjured() || InjuredByDeathCircle)
            {
                if (Actor.AIHealth.HealthPoints < 0.0f)
                {
                    nextState = AIState.Dead;
                }
                else if (InjuredByDeathCircle)
                {
                    InjuredByDeathCircle = false;
                    nextState = AIState.DeathCircle;
                }
                else if (Actor.AIHealth.HealthPoints < LifeFleeThreshold)
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

        private void UpdateWeaponKnowledge()
        {
            ObjectContainedInventory cell = Actor.AIInventory.GetPrimaryWeapon();
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

            Actor.EquipmentManager.SelectWeapon();

            //reload
            //ammunition
            //si pas de munition ou plus de munitions: unequipped weapon
            //essai de reloader
            //essai equiper nouvelle weapon
        }

        private void UpdateProtectionKnowledge()
        {
            Actor.EquipmentManager.SelectHelmet();
            Actor.EquipmentManager.SelectVest();

            vestProtectionRatio = 0.0f;
            helmetProtectionRatio = 0.0f;

            ObjectContainedInventory cellHelmet = Actor.AIInventory.GetHelmet();
            ObjectContainedInventory cellVest = Actor.AIInventory.GetVest();
            Vest equippedVest = null;
            Helmet equippedHelmet = null;
            if (cellVest != null)
            {
                equippedVest = (Vest)cellVest.GetItem();
            }

            if (equippedVest != null)
            {
                hasVestEquipped = true;
                vestProtectionRatio += equippedVest.ProtectionValue;
            }
            else
            {
                hasVestEquipped = false;
            }
            if (cellHelmet != null)
            {
                equippedHelmet = (Helmet)cellHelmet.GetItem();
            }

            if (equippedHelmet != null)
            {
                hasHelmetEquipped = true;
                helmetProtectionRatio += equippedHelmet.ProtectionValue;
            }
            else
            {
                hasHelmetEquipped = false;
            }
            helmetProtectionRatio /= HelmetProtectionMaximum;
            vestProtectionRatio /= VestProtectionMaximum;
        }
        private void UpdateSupportKnowledge()
        {

            //Bag
            Actor.EquipmentManager.SelectBag();
            bagCapacityRatio = 0.0f;
            ObjectContainedInventory cellBag = Actor.AIInventory.GetBag();
            Bag equippedBag = null;

            if (cellBag != null)
            {
                equippedBag = (Bag)cellBag.GetItem();
            }

            if (equippedBag != null)
            {
                hasBagEquipped = true;
                bagCapacityRatio += equippedBag.Capacity;
            }
            else
            {
                hasBagEquipped = false;
            }
            bagCapacityRatio /= BagCapacityMaximum;

            //Boost
            boostNumberStorageRatio = (float)Actor.AIInventory.GetItemQuantityInInventory(ItemType.Boost, AmmoType.None) / MaxUsefulStoredBoostItem;
            boostNumberStorageRatio = Mathf.Clamp(boostNumberStorageRatio, 0.0f, 1.0f);
            //Heal
            healthRatio = Actor.AIHealth.HealthPoints / Actor.AIHealth.MaxHealthPoints;
            healNumberStorageRatio = (float)Actor.AIInventory.GetItemQuantityInInventory(ItemType.Heal, AmmoType.None) / MaxUsefulStoredHealItem;
            healNumberStorageRatio = Mathf.Clamp(healNumberStorageRatio, 0.0f, 1.0f);

            //Ammopack
            for (int i = 0; i < NumberOfAmmoPackType; i++)
            {
                ammoPackNumberStorageRatio[i] = (float)Actor.AIInventory.GetItemQuantityInInventory(ItemType.AmmoPack, (AmmoType)i) / MaxUsefulStoredAmmoPackItem;
                ammoPackNumberStorageRatio[i] = Mathf.Clamp(ammoPackNumberStorageRatio[i], 0.0f, 1.0f);
            }
        }

        public float GetAmmoPackStorageRatio(AmmoType ammoType)
        {
            return ammoPackNumberStorageRatio[(int)ammoType];
        }
        public void SetAmmoPackStorageRatio(AmmoType ammoType, float ammoPackStorageRatio)
        {
            ammoPackNumberStorageRatio[(int)ammoType] = ammoPackStorageRatio;
        }

        private void UpdateInventoryKnowledge()
        {
            //garder meilleur bag, helmet,vest,boost,heal dans l'inventaire
            if (Actor.AIInventory.ListInventory != null)
            {
                Item item = null;
                foreach (ObjectContainedInventory cell in Actor.AIInventory.ListInventory)
                {
                    item = cell.GetItem();

                    switch (item.Type)
                    {
                        case ItemType.Helmet:
                            if (inventoryBestHelmet == null)
                            {
                                InventoryBestHelmet = cell;
                            }
                            else
                            {
                                Helmet helmet = (Helmet)item;
                                if (((Helmet)inventoryBestHelmet.GetItem()).ProtectionValue < helmet.ProtectionValue)
                                {
                                    InventoryBestHelmet = cell;
                                }
                            }
                            break;
                        case ItemType.Vest:
                            if (inventoryBestVest == null)
                            {
                                InventoryBestVest = cell;
                            }
                            else
                            {
                                Vest vest = (Vest)item;
                                if (((Vest)inventoryBestVest.GetItem()).ProtectionValue < vest.ProtectionValue)
                                {
                                    InventoryBestVest = cell;
                                }
                            }
                            break;
                        case ItemType.Bag:
                            if (inventoryBestBag == null)
                            {
                                InventoryBestBag = cell;
                            }
                            else
                            {
                                Bag bag = (Bag)item;
                                if (((Bag)inventoryBestBag.GetItem()).Capacity < bag.Capacity)
                                {
                                    InventoryBestBag = cell;
                                }
                            }
                            break;
                        case ItemType.Heal:
                            if (inventoryBestHeal == null)
                            {
                                InventoryBestHeal = cell;
                            }
                            else
                            {
                                Heal heal = (Heal)item;
                                if (((Heal)inventoryBestHeal.GetItem()).Efficacity < heal.Efficacity)
                                {
                                    InventoryBestHeal = cell;
                                }
                            }
                            break;
                        case ItemType.Boost:
                            if (inventoryBestBoost == null)
                            {
                                InventoryBestBoost = cell;
                            }
                            else
                            {
                                Boost boost = (Boost)item;
                                if (((Boost)inventoryBestBoost.GetItem()).Efficacity < boost.Efficacity)
                                {
                                    InventoryBestBoost = cell;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void UpdateDeathCircleKnowledge(DeathCircleController deathCircleController)
        {
            SafeCircleCenterPosition = deathCircleController.SafeCircle.transform.position;
            DeathCircleCenterPosition = deathCircleController.DeathCircle.transform.position;
            SafeCircleRadius = deathCircleController.SafeCircle.Radius;
            DeathCircleRadius = deathCircleController.DeathCircle.Radius;
            CurrentDeathCircleHurtPoints = deathCircleController.DeathCircleValues.DomagePerSecond[(int)deathCircleController.CurrentPhase];

            Vector2 aiPosition = Vector2.zero;
            Vector2 safeCirclePosition = Vector2.zero;
            aiPosition.x = Actor.transform.position.x;
            aiPosition.y = Actor.transform.position.z;
            safeCirclePosition.x = DeathCircleCenterPosition.x;
            safeCirclePosition.y = DeathCircleCenterPosition.z;
            CurrentDistanceOutsideSafeCircle = Vector2.Distance(aiPosition, safeCirclePosition) - SafeCircleRadius;
            //{

            //    deathCircleIsClosing = true;
            //}
            //else
            //{
            //    deathCircleIsClosing = false;
            //}
        }

        //private bool NeedToEscapeClosingDeathCircle()
        //{
        //    bool needToEscape = false;
        //    if (deathCircleIsClosing)
        //    {
        //        if (CurrentDistanceOutsideSafeCircle > 0.0f)
        //        {
        //            needToEscape = true;
        //        }
        //    }
        //    return needToEscape;
        //}
    }
}