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
            if (other.gameObject.CompareTag(R.S.Tag.Player) || other.gameObject.CompareTag(R.S.Tag.Ai))
            {
                Health health = other.gameObject.GetComponentInChildren<Health>();
                health.Hit(1);
            }
            Destroy(gameObject);
        }
    }
}

