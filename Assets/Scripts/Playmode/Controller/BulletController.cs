using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    //BEN_CORRECTION : Je constate que vous avez aucun "Sensor" en dehors des "Input", ce qui ne me dérange pas vraiment.
    //
    //                 Ce qui me dérange par contre, c'est que cette classe se dit être un contrôleur tandis que c'est en fait
    //                 un "Aspect" (ou 2 aspects pour être exact, je vais y revenir).
    //
    //                 Ce que je veux dire, c'est que je ne vois pas pourquoi ce script ne pourrait pas être réutilisé ailleurs.
    //                 Pour l'instant, de par son nom, vous semblez dire qu'il ne peut servir que vous les "Bullet", ce qui n'est pas vrai.
    //                 Par exemple, s'il y a un feu dans le monde, et que le joueur entre en contact avec le feu, ses points de vie
    //                 devraient diminer. Ici, c'est exactement la même chose : une balle entre en colision avec un joueur, et donc,
    //                 ses points de vie diminuent.
    //
    //                 Ce script fait donc diminuer les points de vie de tous ce qu'il touche. N'est-ce pas merveilleux ? Du moment
    //                 que l'un de vos GameObjects doit faire des dégats au joueur à son contact, vous ne faites qu'ajouter ce script
    //                 au GameObject, le configurer (avec une SerializedField) et c'est tout!
    //
    //                 Par contre, ce script a deux responsabilités : déruire le GameObject au bout d'un certain momment et faire des dégats.
    //                 Il devrait donc être séparé en deux.
   
    
    public class BulletController : NetworkGameScript
    {
        public void SetLivingTime(float livingTime)
        {
            Destroy(gameObject, livingTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            //BEN_REVIEW : Cette ligne est trop longue. Faire une retour de chariot après le ||
            if (other.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Player) || other.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Ai))
            {
                Health health = other.gameObject.GetComponentInChildren<Health>();
                if (health != null)
                {
                    //BEN_CORRECTION : 1 ? Cette valeur devrait être un "SerializedField".
                    health.Hit(1);
                }
            }
            Destroy(gameObject);
        }
    }
}

