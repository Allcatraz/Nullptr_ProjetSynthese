using UnityEngine;


namespace ProjetSynthese
{
    public class WaterColliderHandler : GameScript
    {

        private void OnTriggerEnter(Collider col)
        {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsSwimming == false)
            {
                playerController.IsSwimming = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsSwimming == true)
            {
                playerController.IsSwimming = false;
            }
        }
    }
}
