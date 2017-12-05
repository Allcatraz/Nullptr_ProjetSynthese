using Harmony;
using UnityEngine;


namespace ProjetSynthese
{
    public class WaterColliderHandler : GameScript
    {

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Water) && gameObject.tag == R.S.Tag.Map)
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
        }

        private void OnTriggerExit2D(Collider2D col)
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
