
using UnityEngine;

namespace ProjetSynthese
{
    public class AIRadar
    {
        private const float NoRangePerception = 0.0f;

        private const float LowRangePerception = 5.0f;
        private const float MediumRangePerception = 10.0f;
        private const float HighRangePerception = 15.0f;

        private float currentPerceptionRange = LowRangePerception;
        private float circleCastDistance = 0.0f;
        private Vector3 circleCastDirection = Vector3.up;

        public enum PerceptionLevel { None, Low, Medium, High };

        private PerceptionLevel aiPerceptionLevel;
        public PerceptionLevel AIPerceptionLevel
        {
            get
            {
                return aiPerceptionLevel;
            }
            set
            {
                aiPerceptionLevel = value;
                switch (aiPerceptionLevel)
                {
                    case PerceptionLevel.None:
                        currentPerceptionRange = NoRangePerception;
                        break;
                    case PerceptionLevel.Low:
                        currentPerceptionRange = LowRangePerception;
                        break;
                    case PerceptionLevel.Medium:
                        currentPerceptionRange = MediumRangePerception;
                        break;
                    case PerceptionLevel.High:
                        currentPerceptionRange = HighRangePerception;
                        break;
                    default:
                        break;
                }
            }
        }

        //BEN_CORRECTION : Pourquoi ne pas avoir utilisé R.E.Layer ?
        public enum LayerType { None, Default, Item, EquippedItem,Player, AI, Building }

        //BEN_CORRECTION : Pourquoi ne pas avoir utilisé R.S.Layer ?
        public static readonly string[] LayerNames = { "None", "Default", "Item", "EquippedItem", "Player", "AI", "Building" };

        public AIRadar()
        {
            AIPerceptionLevel = PerceptionLevel.Low;
            currentPerceptionRange = LowRangePerception;
        }

        public ObjectType NeareastGameObject<ObjectType>(Vector3 position, LayerType layerType) /* where ObjectType : class */
        {
            //BEN_REVIEW : "default", en C#, retourne la valeur par défaut pour un type donné. Si c'est un type de base (tel que int), retourne
            //             sa valeur par défaut (0 pour int, false pour bool, etc...). Dans le cas d'un type "référence" (n'importe quel objet quoi),
            //             cela retourne "null".
            //
            //             Dans votre cas, ce sera toujours un objet, donc vous pouvez directement mettre "null" à la place.
            //             Par contre, ça va créer une erreur à la compilation et c'est là que le mot clé "where" entre en jeu (voir ce que j'ai mis
            //             en commentaire après le nom de la fonction).
            //
            //             Ce que je veux dire ici :
            //              1. default est assez lent.
            //              2. default est peu utilisé et peu porter à confusion.
            //              3. default, dans ce cas présent, retournera toujours null.
            ObjectType nearestObject = default(ObjectType);
            RaycastHit[] inRangeObjects;
            if (layerType == LayerType.None)
            {
                inRangeObjects = Physics.SphereCastAll(position, currentPerceptionRange, circleCastDirection);
            }
            else
            {
                LayerMask layerMask = GetLayerMask(layerType);
                inRangeObjects = Physics.SphereCastAll(position, currentPerceptionRange, circleCastDirection, circleCastDistance, layerMask);
            }

            int neareastItemIndex = -1;
            float smallestDistance = float.MaxValue;
            if (inRangeObjects != null)
            {
                for (int i = 0; i < inRangeObjects.Length; i++)
                {
                    if (inRangeObjects[i].distance < smallestDistance)
                    {
                        smallestDistance = inRangeObjects[i].distance;
                        neareastItemIndex = i;
                    }
                }
            }
            if (neareastItemIndex != -1)
            {
                nearestObject = inRangeObjects[neareastItemIndex].collider.gameObject.GetComponentInParent<ObjectType>();
            }
            return nearestObject;
        }

        public ActorAI NeareastNonAllyAI(ActorAI selfAI)
        {
            Vector3 position = selfAI.transform.position;
            ActorAI nearestNonAllyAI = default(ActorAI);
            RaycastHit[] inRangeObjects;
            LayerType layerType = LayerType.AI;
            LayerMask layerMask = GetLayerMask(layerType);
            inRangeObjects = Physics.SphereCastAll(position, currentPerceptionRange, circleCastDirection, circleCastDistance, layerMask);
           
            int neareastItemIndex = -1;
            float smallestDistance = float.MaxValue;
            if (inRangeObjects != null)
            {
                for (int i = 0; i < inRangeObjects.Length; i++)
                {
                    if (inRangeObjects[i].distance < smallestDistance && !(Object.ReferenceEquals(selfAI, inRangeObjects[i].collider.gameObject.GetComponentInParent<ActorAI>())))
                    {
                        smallestDistance = inRangeObjects[i].distance;
                        neareastItemIndex = i;
                    }
                }
            }
            if (neareastItemIndex != -1)
            {
                nearestNonAllyAI = inRangeObjects[neareastItemIndex].collider.gameObject.GetComponentInParent<ActorAI>();
            }
            return nearestNonAllyAI;
        }

        private RaycastHit[] AllNeareastGameObjects(Vector3 position, LayerType layerType)
        {
            RaycastHit[] inRangeObjects;
            if (layerType == LayerType.None)
            {
                inRangeObjects = Physics.SphereCastAll(position, currentPerceptionRange, circleCastDirection);
            }
            else
            {
                LayerMask layerMask = GetLayerMask(layerType);
                inRangeObjects = Physics.SphereCastAll(position, currentPerceptionRange, circleCastDirection, circleCastDistance, layerMask);
            }
            
            return inRangeObjects;
        }

        private int GetLayerMask(LayerType layerType)
        {

            LayerMask layerMask = LayerMask.NameToLayer(LayerNames[(int)layerType]);
            layerMask = 1 << layerMask;

            return layerMask;
        }

        public Item NeareastNonEquippedItem(Vector3 position)
        {
            RaycastHit[] itemsInPerceptionRange = AllNeareastGameObjects(position, AIRadar.LayerType.Item);
            Item nonEquippedNearestItem = null;
            int nonEquippedNeareastItemIndex = -1;
            float smallestDistance = float.MaxValue;
            if (itemsInPerceptionRange != null)
            {
                for (int i = 0; i < itemsInPerceptionRange.Length; i++)
                {
                    if (itemsInPerceptionRange[i].distance < smallestDistance)
                    {
                        smallestDistance = itemsInPerceptionRange[i].distance;
                        nonEquippedNeareastItemIndex = i;
                    }
                }
            }
            if (nonEquippedNeareastItemIndex != -1)
            {
                nonEquippedNearestItem = itemsInPerceptionRange[nonEquippedNeareastItemIndex].collider.gameObject.GetComponentInParent<Item>();
            }
            return nonEquippedNearestItem;

        }

        public bool IsGameObjectHasLineOfSight(Vector3 position, PlayerController target)
        {         
            Vector3 direction = Vector3.zero;
            direction = target.transform.position - position;          
            return Physics.Raycast(position, direction, currentPerceptionRange);
        }
        //Nécessaire pour disinguer AI opponent et AI ally research vs player
        //plus rapide à cause du ou dans le if de décision ailleurs d'avaoir deux fonctions
        //Évite aussi des vérification de type et casting lents
        
        public bool IsGameObjectHasLineOfSight(Vector3 position, ActorAI target)
        {
            Vector3 direction = Vector3.zero;
            direction = target.transform.position - position;
            return Physics.Raycast(position, direction, currentPerceptionRange);
        }


    }
}