using System.Collections;
using System.Collections.Generic;
using Harmony;
using ProjetSynthese;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    //BEN_CORRECTION : Ça m'a pris un bon 2 minutes pour comprendre que c'est relié à l'interface utilisateur!
    //                 Et ça s'appelle Controlleur....
    //
    //                 Pourquoi pas DistanceFromDeathCircleView ?
    
    public class DistanceDeathCircleController : GameScript
    {
        private RectTransform rectTransform;
        private Image runnerImage;
        private DeathCircleDistanceEventChannel deathCircleDistanceEventChannel;

        private void InjectDistanceDeathCircleController([EventChannelScope] DeathCircleDistanceEventChannel deathCircleDistanceEventChannel)
        {
            this.deathCircleDistanceEventChannel = deathCircleDistanceEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDistanceDeathCircleController");
            deathCircleDistanceEventChannel.OnEventPublished += OnDistanceChanged;

            runnerImage = GetAllChildrens()[0].GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnDistanceChanged(DeathCircleDistanceEvent deathCircleDistanceEvent)
        {
            float playerDistance = deathCircleDistanceEvent.PlayerRadius - deathCircleDistanceEvent.SafeCircleRadius;
            float circleDistance = deathCircleDistanceEvent.DeathCircleRadius - deathCircleDistanceEvent.SafeCircleRadius;
            float distanceLeft = 0;

            if (circleDistance > 0)
            {
                distanceLeft = playerDistance / circleDistance;
            }

            if (distanceLeft <= 0 && playerDistance <= 0)
            {
                ComputeDistance(0);
                SetAlpha(0.5f);
            }
            else if (distanceLeft <= 0 && playerDistance >= 0  || distanceLeft >= 1 && playerDistance >= 0)
            {
                ComputeDistance(1);
                SetAlpha(1f);
            }
            else
            {
                ComputeDistance(distanceLeft);
                SetAlpha(1f);
            }
        }

        private void SetAlpha(float alpha)
        {
            Color temp = runnerImage.color;
            temp.a = alpha;
            runnerImage.color = temp;
        }

        private void ComputeDistance(float distanceLeft)
        {
            distanceLeft = rectTransform.sizeDelta.x * distanceLeft;
            runnerImage.transform.position = new Vector3(distanceLeft, runnerImage.transform.position.y, runnerImage.transform.position.z);
        }
    }
}