using System.Collections;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    //BEN_CORRECTION : Ceci n'est pas un Effect, mais il se trouve dans "Effect".
    //
    //                 Je pense que ceci est un Actuator (pour le mouvement de la porte) et aussi un aspect (pour
    //                 déclancher le mouvement de la porte).
    
    public class OpenDoor : GameScript
    {
        [Tooltip("Le visuel de la porte.")]
        [SerializeField] private Transform visualTransform;
        [Tooltip("Nombre de degrés pour ouvrir la porte.")]
        [SerializeField] private float openedAngle;
        [Tooltip("Nombre de degrés pour fermer la porte.")]
        [SerializeField] private float closedAngle;

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
                float angleToDo = Mathf.Clamp(Mathf.Abs(openedAngle - (startAngle - currentangle)), closedAngle, openedAngle);
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
                float angleToDo = Mathf.Clamp(startAngle - currentangle, closedAngle, openedAngle);
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
