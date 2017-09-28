﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRadar
{
    private const float NoRangePerception = 0.0f;

    [SerializeField]
    private const float LowRangePerception = 50.0f;
    [SerializeField]
    private const float MediumRangePerception = 100.0f;
    [SerializeField]
    private const float HighRangePerception = 150.0f;

    private float currentPerceptionRange = LowRangePerception;
    private float circleCastDistance = 0.0f;
    private Vector3 circleCastDirection = Vector3.up;

    public enum PerceptionLevel { None,Low, Medium, High };

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

    public void Init()
    {
        AIPerceptionLevel = PerceptionLevel.Low;
        currentPerceptionRange = LowRangePerception;
    }


    public ObjectType NeareastGameObject<ObjectType>(Vector3 position, int layerMask)
    {

        ObjectType nearestObject = default(ObjectType);
        RaycastHit[] inRangeObjects;
        inRangeObjects = Physics.SphereCastAll(position, currentPerceptionRange, circleCastDirection, circleCastDistance, layerMask);
        //inRangeObjects = Physics.SphereCastAll(position, currentPerceptionRange, circleCastDirection);
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
