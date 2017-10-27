using UnityEngine;

namespace ProjetSynthese
{
    //BEN_CORRECTION : J'en ai parlé avec Frédérick. Il connait mes commentaires à ce sujet
    //
    //                 Mais pour les autres, voici ce dont nous avons conclus en inspectant l'architecture actuelle et comment
    //                 l'améliorer pour le futur.
    //
    //                 Ce que nous avons ici ressemble énormément à une architecture de type "Blackboard". Dans une 
    //                 telle architecture, il y a :
    //
    //                    1. Un "Blackboard", qui est en fait un dépot d'informations. Ici, c'est "AiBrain".
    //                    2. Des "Knowlegdge Sources", qui mettent les informations dans le "BlackBoard" à jour.
    //                       Ici, AiRadar en est un.
    //                    3. Des "Control Components", qui prennent des décisions en fonction des informations dans le "Blackboard".
    //                       Ici, ce sont les "States".
    //
    //                 Dans cette implémentation, "AiBrain" est un dépôt d'informations variées, telles que les ennemis visibles,
    //                 les items rencontrés, le pourcentage de points de vie restant et ainsi de suite. Il est ensuite possible de
    //                 questionner "AiBrain" sur ces informations, tel que "Quel est l'ennemi le plus proche ?" ou "Suis-je sur le
    //                 point de mourir ?". "AiBrain" ne prends aucune décision : il ne fait que contenir l'information et la transformer
    //                 en d'autres informations.
    //                 
    //                 Viennent ensuite les sources d'information. Il y aura une classe par type d'information, tel que les points de vie,
    //                 la position des ennemis, la quantité de munitions restantes et ainsi de suite. Elles sont très finement découpées,
    //                 de sortes à ce que l'ajout d'une source d'information ne consiste qu'en la création d'une nouvelle classe. Encore
    //                 une fois, elles ne prennent aucune décision : elles ne font que mettre à jour les informations dans "AiBrain".
    //
    //                 Enfin vient les composants de contrôle. C'est ici que cela diffère le plus du pattern "Blackboard", car on vient
    //                 y implanter les "States". Il y a, pour commencer, un "Maitre". Ce dernier sert à décider quel est le "State" actuel
    //                 en fonction des informations dans "AiBrain". Pour l'instant, cela fait partie de "AiBrain", mais cela sera extrait
    //                 dans une classe à part dans le futur (ActorAi peut-être...). C'est le premier niveau de prise de décision. 
    //                 
    //                 Il y a ensuite les "States". Les "States" aident à prendre des décisions de manière plus précise. Par exemple, si
    //                 l'AI est dans l'état Attack, ce dernier devra prendre la décision sur l'arme à utiliser (toujours en fonction de
    //                 l'information contenue dans "AiBrain", telle que le nombre de balles dans le chargeur actuel).
    //
    //                 Pour effectuer une action, les "States" parleront directement avec "AiController", qui lui, parlera directement
    //                 avec le composant dédié à une tâche précise (tel que effectuer le déplacement). Par exemple, si l'AI est dans 
    //                 l'état "Hunt", et que le "State" décide de se déplacer vers un Item, alors il demandera à "AiController" de se déplacer
    //                 vers une position précise. "AiController" demandera ensuite à un composant "AiMover" d'effectuer le déplacement 
    //                 vers ce point.
    //
    //                 Le but de cette architecture est double :
    //                    1. Conserver le maximum possible de ce qui existe déjà.
    //                    2. Créer quelque chose le plus près possible d'une architecture d'AI « Goal Driven ».
    //
    //                 Bref, comme c'est WIP, j'ai moins pénalisé, mais j'ai tout de même enlevé des points pour tout ce qui est vraiment
    //                 innaceptable.
    public class AIBrain
    {
        //BEN_CORRECTION : Il y a vraiment beaucoup de problèmes de formattage ici, entre autres un problème d'espacement vertical
        //                 et de regroupement des éléments.
        
        #region Parameters
        private const float LifeFleeThresholdFactor = 0.20f;
        private readonly float LifeFleeThreshold;
        private float lastLifePointLevel;
        private const float ErrorLifeTolerance = 0.001f;
        private const float ProtectionMaximum = 100.0f;

        private readonly ActorAI Actor;
        public readonly GoalEvaluator goalEvaluator;

        public enum OpponentType { None, AI, Player };
        #endregion

        #region Knowledge
        private float healthRatio = 1.0f;
        
        private float protectionRatio = 0.0f;
        
        public float HealthRatio { get { return healthRatio; } private set { healthRatio = value; } }
        public float ProtectionRatio { get { return protectionRatio; } private set { protectionRatio = value; } }
       
        //BEN_CORRECTION : Utilise des propriétés automatiques quand c'est possible.
        //                 https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties
        private bool hasPrimaryWeaponEquipped = false;
        private bool hasHelmetEquipped = false;
        private bool hasVestEquipped = false;
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
        }

        public AIState WhatIsMyNextState(AIState currentState)
        {
            AIState nextState = currentState;
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
                if (Actor.AIHealth.HealthPoints < 0.0f)
                {
                    nextState = AIState.Dead;
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
            protectionRatio = 0.0f;
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
                protectionRatio += equippedVest.ProtectionValue;
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
                protectionRatio +=equippedHelmet.ProtectionValue;
            }
            else
            {
                hasHelmetEquipped = false;
            }

            protectionRatio /= ProtectionMaximum;
            //choix best vest et best helmet
            
        }
        private void UpdateSupportKnowledge()
        {
            //bag managment
            //heal management
            healthRatio = Actor.AIHealth.HealthPoints/Actor.AIHealth.MaxHealthPoints;
        }
    }
}