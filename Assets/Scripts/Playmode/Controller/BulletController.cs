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
                if (health != null)
                {
                    health.Hit(1);
                }
            }
            Destroy(gameObject);
        }
    }
}

