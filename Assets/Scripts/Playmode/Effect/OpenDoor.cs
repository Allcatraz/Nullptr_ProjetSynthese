using System;
using System.Collections;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class OpenDoor : GameScript
    {
        [Tooltip("Le visuel de la porte.")]
        [SerializeField] private Transform visualTransform;
        [Tooltip("Nombre de degrés pour ouvrir la porte.")]
        [SerializeField] private float angle;

        private PlayerUseEventChannel playerUseEventChannel;

        private float startAngle = 0;
        private bool isPlayerInRange = false;
        private bool isDoorOpen = false;
        private bool isCoroutineRunning = false;

        private void InjectOpenDoor([EventChannelScope] PlayerUseEventChannel playerUseEventChannel)
        {
            this.playerUseEventChannel = playerUseEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectOpenDoor");
            playerUseEventChannel.OnEventPublished += OnPlayerUse;
            startAngle = visualTransform.localEulerAngles.y;
            if (startAngle == 0)
            {
                startAngle = 360;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            isPlayerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            isPlayerInRange = false;
        }

        private void OnPlayerUse(PlayerUseEvent playerUseEvent)
        {
            if (isPlayerInRange)
            {                  
                if (isCoroutineRunning)
                {
                    StopCoroutine("ComputeOpenDoor");
                }
                StartCoroutine("ComputeOpenDoor");
                isDoorOpen = !isDoorOpen;
            }
        }

        IEnumerator ComputeOpenDoor()
        {
            if (!isCoroutineRunning)
            {
                isCoroutineRunning = !isCoroutineRunning;
            }

            float currentangle = visualTransform.localEulerAngles.y;
            if (Math.Abs(currentangle) < 0.1f)
            {
                currentangle = 360;
            }
            if (!isDoorOpen)
            {
                float angleToDo = Mathf.Clamp(angle - (startAngle - currentangle), 0, angle);
                for (float i = 0; i <= angleToDo; i+=2f)
                {
                    visualTransform.localEulerAngles = new Vector3(visualTransform.transform.localEulerAngles.x, currentangle - i, visualTransform.transform.localEulerAngles.z);
                    yield return new WaitForSeconds(0.000001f);
                }
                isCoroutineRunning = false;
                yield return null;
            }
            else
            {
                float angleToDo = Mathf.Clamp(startAngle - currentangle, 0, angle);
                for (float i = 0; i <= angleToDo; i+=2f)
                {
                    visualTransform.localEulerAngles = new Vector3(visualTransform.transform.localEulerAngles.x, currentangle + i, visualTransform.transform.localEulerAngles.z);
                    yield return new WaitForSeconds(0.000001f);
                }
                isCoroutineRunning = false;
                yield return null;
            }
        }
    }
}
