using System;
using Boo.Lang;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class WaterColliderHandler : GameScript
    {
        private List<Collision> listPlayerCollision;

        private void OnCollisionEnter(Collision col)
        {
            if (listPlayerCollision == null)
            {
                listPlayerCollision = new List<Collision>();
            }
            if (!CheckIfExistInList(col)) listPlayerCollision.Add(col);
        }

        private bool CheckIfExistInList(Collision col)
        {
            bool exist = false;
            foreach (Collision colli in listPlayerCollision)
            {
                if (colli == col)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        private void FixedUpdate()
        {
            if (listPlayerCollision != null)
            {
                foreach (Collision playerCollider in listPlayerCollision)
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
    }
}