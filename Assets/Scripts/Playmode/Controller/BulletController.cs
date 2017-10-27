using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    public class BulletController : NetworkGameScript
    {
        private float dommage = 0;

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
                PlayerController playerController = other.gameObject.GetComponentInChildren<PlayerController>();                
                if (health != null && playerController.isLocalPlayer)
                {
                    Item[] protectionItems = playerController.GetInventoryProtection();
                    float helmetProtection = protectionItems[0] == null ? 0 : ((Helmet) protectionItems[0]).ProtectionValue;
                    float vestProtection = protectionItems[1] == null ? 0 : ((Vest) protectionItems[1]).ProtectionValue;
                    health.Hit(dommage - (dommage * (helmetProtection + vestProtection) / 100));
                }
            }
            Destroy(gameObject);
        }
    }
}

