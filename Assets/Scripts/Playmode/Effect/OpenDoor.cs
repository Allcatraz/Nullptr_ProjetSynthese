using System.Collections;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class OpenDoor : GameScript
    {
        [SerializeField] private Transform visualTransform;
        [SerializeField] private float openedAngle;
        [SerializeField] private float closedAngle;

        private PlayerUseEventChannel playerUseEventChannel;

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
                isDoorOpen = !isDoorOpen;
                if (isCoroutineRunning)
                {
                    StopCoroutine("ComputeOpenDoor");
                }
                StartCoroutine("ComputeOpenDoor");
            }
        }

        IEnumerator ComputeOpenDoor()
        {
            if (!isCoroutineRunning)
            {
                isCoroutineRunning = !isCoroutineRunning;
            }

            float currentangle = visualTransform.localEulerAngles.y; 
            if (isDoorOpen)
            {
                float angleToDo = Mathf.Clamp(Mathf.Abs(openedAngle - (360 - currentangle)), closedAngle, openedAngle);
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
                float angleToDo = Mathf.Clamp(360 - currentangle, closedAngle, openedAngle);
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
