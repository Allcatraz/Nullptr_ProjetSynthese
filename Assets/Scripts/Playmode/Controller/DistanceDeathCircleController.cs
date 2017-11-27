using System;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class DistanceDeathCircleController : GameScript
    {
        private RectTransform rectTransform;
        private Image runnerImage;
        private DeathCircleDistanceEventChannel deathCircleDistanceEventChannel;
        private PlayerMoveEventChannel playerMoveEventChannel;

        private float circleDistance;
        private float safeCircleRadius;
        private float playerDistance;
        private float playerRadius;
        private Vector3 playerPosition;
        private Vector3 center;

        private void InjectDistanceDeathCircleController([EventChannelScope] DeathCircleDistanceEventChannel deathCircleDistanceEventChannel,
                                                         [EventChannelScope] PlayerMoveEventChannel playerMoveEventChannel)
        {
            this.playerMoveEventChannel = playerMoveEventChannel;
            this.deathCircleDistanceEventChannel = deathCircleDistanceEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectDistanceDeathCircleController");
            deathCircleDistanceEventChannel.OnEventPublished += OnDistanceChanged;
            playerMoveEventChannel.OnEventPublished += OnPlayerMove;

            runnerImage = GetAllChildrens()[0].GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnPlayerMove(PlayerMoveEvent playerMoveEvent)
        {
            playerPosition = playerMoveEvent.PlayerMover.transform.position;
            playerRadius = Mathf.Sqrt(Mathf.Pow(center.x - playerPosition.x, 2) + Mathf.Pow(center.z - playerPosition.z, 2));
            playerDistance = playerRadius - safeCircleRadius;

            UpdateDistance();            
        }

        private void OnDistanceChanged(DeathCircleDistanceEvent deathCircleDistanceEvent)
        {
            center = deathCircleDistanceEvent.Center;
            safeCircleRadius = deathCircleDistanceEvent.SafeCircleRadius;
            circleDistance = deathCircleDistanceEvent.DeathCircleRadius - deathCircleDistanceEvent.SafeCircleRadius;

            UpdateDistance();
        }

        private void UpdateDistance()
        {
            float distanceLeft = 0;

            if (circleDistance > 0)
            {
                distanceLeft = playerDistance / circleDistance;
            }

            if (playerRadius <= safeCircleRadius || safeCircleRadius == 0)
            {
                ComputeDistance(0);
                SetAlpha(0.5f);
            }
            else if (playerRadius > safeCircleRadius && playerDistance >= circleDistance)
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
            if (Math.Abs(distanceLeft - 1) < 0.5)
            {
                runnerImage.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
            }
            else
            {
                runnerImage.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            }

            distanceLeft = rectTransform.sizeDelta.x * distanceLeft;
            runnerImage.transform.position = new Vector3(distanceLeft, runnerImage.transform.position.y, runnerImage.transform.position.z);         
        }
    }
}