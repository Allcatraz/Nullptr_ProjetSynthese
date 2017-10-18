using System.Collections;
using System.Collections.Generic;
using Harmony;
using ProjetSynthese;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
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
            if (deathCircleDistanceEvent.DeathCircleRadius - deathCircleDistanceEvent.SafeCircleRadius > 0)
            {
                float pourcentage =
                    (deathCircleDistanceEvent.PlayerRadius - deathCircleDistanceEvent.SafeCircleRadius) /
                    (deathCircleDistanceEvent.DeathCircleRadius - deathCircleDistanceEvent.SafeCircleRadius);
                runnerImage.transform.position = new Vector3(runnerImage.transform.position.x + rectTransform.sizeDelta.x * pourcentage,
                                                             runnerImage.transform.position.y, 
                                                             runnerImage.transform.position.z);

                runnerImage.transform.position = new Vector3((runnerImage.transform.position.x < transform.position.x
                                                              || runnerImage.transform.position.x < transform.position.x + rectTransform.sizeDelta.x ? 0: 1), 
                                                              runnerImage.transform.position.y, 
                                                              runnerImage.transform.position.z);
            }
            else
            {
                
            }
        }
    }
}