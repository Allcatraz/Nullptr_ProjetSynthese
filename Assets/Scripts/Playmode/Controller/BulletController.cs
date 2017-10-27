using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    public class BulletController : NetworkGameScript
    {
        public void SetLivingTime(float livingTime)
        {
            Destroy(gameObject, livingTime);
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
                    health.Hit(1);
                }
            }
            Destroy(gameObject);
        }
    }
}

