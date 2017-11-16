using UnityEngine;
using Harmony;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class BulletController : GameScript
    {
        private float dommage = 10;

        public void SetLivingTime(float livingTime)
        {
            Destroy(gameObject, livingTime);
        }

        public void SetDommage(float dommage)
        {
            this.dommage = dommage;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Player) || other.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Ai))
            {
                Health health = other.gameObject.GetComponentInChildren<Health>();

                //Check for AI
                IProtection protectionController = other.gameObject.GetComponentInChildren<PlayerController>() as IProtection;
                if (protectionController == null)
                {
                    protectionController = other.gameObject.GetComponentInChildren<ActorAI>() as IProtection;
                }
                if (health != null && other.gameObject.GetComponent<NetworkBehaviour>().isLocalPlayer)
                {
                    Item[] protectionItems = protectionController.GetInventoryProtection();
                    float helmetProtection = protectionItems[0] == null ? 0 : ((Helmet) protectionItems[0]).ProtectionValue;
                    float vestProtection = protectionItems[1] == null ? 0 : ((Vest) protectionItems[1]).ProtectionValue;
                    health.Hit(10); //dommage - (dommage * (helmetProtection + vestProtection) / 100));
                }
            }
            Destroy(gameObject);
        }
    }
}

