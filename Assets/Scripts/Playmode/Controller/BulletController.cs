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


        public void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == R.S.Tag.Player)
            {
                // TODO Ajouter le code pour gèrer les dégâts au player
                throw new System.NotImplementedException("TODO Ajouter le code pour gèrer les dégâts au player");
            }
            Destroy(gameObject);
        }
    }
}

