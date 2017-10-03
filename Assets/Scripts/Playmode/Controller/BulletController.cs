using System.Collections;
using System.Collections.Generic;
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
            if (other.gameObject.CompareTag(R.S.Tag.Player))
            {
                GameObject hit = other.gameObject;
                Health health = hit.GetComponentInChildren<Health>();
                health.Hit(1);
            }
            Destroy(gameObject);
        }
    }
}

