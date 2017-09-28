using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRadar
{

    [SerializeField]
    private const float LowRangePerception = 50.0f;
    [SerializeField]
    private const float MediumRangePerception = 100.0f;
    [SerializeField]
    private const float HighRangePerception = 150.0f;

    private float currentPerceptionRange = LowRangePerception;
    private float circleCastDistance = 0.0f;
    private Vector2 circleCastDirection = Vector2.zero;

    public enum PerceptionLevel { Low, Medium, High };
    public PerceptionLevel AIPerceptionLevel
    {
        get
        {
            return AIPerceptionLevel;
        }
        set
        {
            AIPerceptionLevel = value;
            switch (AIPerceptionLevel)
            {
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

    public void Init()
    {
        AIPerceptionLevel = PerceptionLevel.Low;
        currentPerceptionRange = LowRangePerception;
    }


    public ObjectType NeareastGameObject<ObjectType>(Vector3 position, int layerMask)
    {

        ObjectType nearestObject = default(ObjectType);
        RaycastHit2D[] inRangeObjects;
        inRangeObjects = Physics2D.CircleCastAll(position, currentPerceptionRange, circleCastDirection, circleCastDistance, layerMask);
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
            nearestObject = inRangeObjects[neareastItemIndex].collider.gameObject.GetComponent<ObjectType>();
        }
        return nearestObject;
    }
}
