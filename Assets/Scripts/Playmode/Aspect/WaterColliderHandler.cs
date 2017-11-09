using UnityEngine;


namespace ProjetSynthese
{
    public class WaterColliderHandler : GameScript
    {

        private void OnTriggerEnter(Collider col)
        {
            ISwim actorController = col.gameObject.GetComponent<PlayerController>() as ISwim;
            if (actorController == null)
            {
                actorController = col.gameObject.GetComponentInParent<ActorAI>() as ISwim;
            }
            if (actorController != null && actorController.IsSwimming == false)
            {
                actorController.IsSwimming = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            ISwim actorController = col.gameObject.GetComponent<PlayerController>() as ISwim;
            if (actorController == null)
            {
                actorController = col.gameObject.GetComponentInParent<ActorAI>() as ISwim;
            }
            if (actorController != null && actorController.IsSwimming == true)
            {
                actorController.IsSwimming = false;
            }
        }
    }
}
