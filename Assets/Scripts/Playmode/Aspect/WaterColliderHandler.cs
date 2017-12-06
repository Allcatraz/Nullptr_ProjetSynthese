using System;
using Boo.Lang;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class WaterColliderHandler : GameScript
    {
        private Collision playerCollider;

        private void OnCollisionEnter(Collision col)
        {
            playerCollider = col;
        }

        private void FixedUpdate()
        {            
            if (playerCollider != null)
            {
                RaycastHit info;
                Vector3 origin = playerCollider.gameObject.transform.position;
                origin.y += 3;
                bool isTrigger = Physics.Raycast(origin, Vector3.down, out info, 10, 1 << LayerMask.NameToLayer(R.S.Layer.Water));
                Debug.DrawRay(origin, Vector3.down, Color.red);
                if (isTrigger)
                {
                    ISwim actorController = playerCollider.gameObject.GetComponent<PlayerController>();
                    if (actorController == null)
                    {
                        actorController = playerCollider.gameObject.GetComponentInParent<ActorAI>();
                    }
                    if (actorController != null && actorController.IsSwimming == false)
                    {
                        actorController.IsSwimming = true;
                    }
                }
                else
                {
                    ISwim actorController = playerCollider.gameObject.GetComponent<PlayerController>();
                    if (actorController == null)
                    {
                        actorController = playerCollider.gameObject.GetComponentInParent<ActorAI>();
                    }
                    if (actorController != null && actorController.IsSwimming)
                    {
                        actorController.IsSwimming = false;
                    }
                }
            }
        }
    }
}