using UnityEngine;
using Harmony;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class BulletController : GameScript
    {
        public int Dommage { get; set; }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Player) || other.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Ai))
            {
                bool isAI = false;
                Health health = other.gameObject.GetComponentInChildren<Health>();

                IInventory inventoryController = other.gameObject.GetComponentInChildren<PlayerController>();        
                if (inventoryController == null)
                {
                    inventoryController = other.gameObject.GetComponentInChildren<ActorAI>();
                    isAI = true;
                }
                if (health != null)
                {
                    Item[] protectionItems = inventoryController.GetProtections();
                    float helmetProtection = protectionItems[0] == null ? 0 : ((Helmet) protectionItems[0]).ProtectionValue;
                    float vestProtection = protectionItems[1] == null ? 0 : ((Vest) protectionItems[1]).ProtectionValue;
                    health.Hit(Dommage - (Dommage * ((helmetProtection + vestProtection) / 100)),  isAI);
                }
            }
            Destroy(gameObject);
        }
    }
}

