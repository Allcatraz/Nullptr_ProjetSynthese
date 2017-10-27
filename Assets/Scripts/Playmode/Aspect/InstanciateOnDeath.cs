using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    //BEN_REVIEW : Ne semble pas servir nulle part
    
    //BEN_CORRECTION : À part mes scripts, votre projet n'a aucun Aspect, ce qui veut dire que vous considirez
    //                 qu'il y a aucun comportement réutilisable dans votre projet (du genre, drop un item quand 
    //                 on meurt).
    //
    //                 Ça sent pas bon. J'ai pas hâte de voir vos contrôleurs, ou devrais-je dire
    //                 « God Class » (https://en.wikipedia.org/wiki/God_object).

    
    [AddComponentMenu("Game/Aspect/InstanciateOnDeath")]
    public class InstanciateOnDeath : GameScript
    {
        [SerializeField]
        private GameObject prefab;

        private Transform topParentTranform;
        private Health health;

        private void InjectInstanciateOnDeath([TopParentScope] Transform topParentTranform,
                                            [EntityScope] Health health)
        {
            this.topParentTranform = topParentTranform;
            this.health = health;
        }

        private void Awake()
        {
            InjectDependencies("InjectInstanciateOnDeath");
        }

        private void OnEnable()
        {
            health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            health.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            Instantiate(prefab, topParentTranform.position, Quaternion.Euler(Vector3.zero));
        }
    }
}