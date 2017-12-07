using System;
using Boo.Lang;
using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace ProjetSynthese
{
    public class WaterColliderHandler : GameScript
    {
        private HashSet<GameObject> listPlayerCollision;

        private void OnCollisionEnter(Collision col)
        {
            if (listPlayerCollision == null)
            {
                listPlayerCollision = new HashSet<GameObject>();
            }
            listPlayerCollision.Add(col.gameObject);
        }

        private void FixedUpdate()
        {
            if (listPlayerCollision != null)
            {
                foreach (GameObject playerCollider in listPlayerCollision)
                {
                    if (playerCollider)
                    {
                        RaycastHit info;
                        Vector3 origin = playerCollider.transform.position;
                        origin.y += 3;
                        bool isTrigger = Physics.Raycast(origin, Vector3.down, out info, 10, 1 << LayerMask.NameToLayer(R.S.Layer.Water));
                        Debug.DrawRay(origin, Vector3.down, Color.red);
                        if (isTrigger)
                        {
                            ISwim actorController = playerCollider.GetComponent<PlayerController>();
                            if (actorController == null)
                            {
                                actorController = playerCollider.GetComponentInParent<ActorAI>();
                            }
                            if (actorController != null && actorController.IsSwimming == false)
                            {
                                actorController.IsSwimming = true;
                            }
                        }
                        else
                        {
                            ISwim actorController = playerCollider.GetComponent<PlayerController>();
                            if (actorController == null)
                            {
                                actorController = playerCollider.GetComponentInParent<ActorAI>();
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
    }
}