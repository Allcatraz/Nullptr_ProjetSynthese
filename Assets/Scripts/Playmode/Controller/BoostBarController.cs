using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    //BEN_CORRECTION : C'est pas une "View" ça ?
    //                 Ou alors, devrait-il exister une "BoostBarView" aussi ?
    //
    //                 J'en suis juste à la deuxième classe de "Controller" et je pense que certains
    //                 de vos contrôleurs ne sont pas des contrôleurs mais des aspects ou des views.
    //
    //                 
    
    public class BoostBarController : GameScript
    {
        [Tooltip("Visuel de la bar de boost.")]
        [SerializeField] private Image barVisual;

        private BoostStats boostStats;
        private PlayerBoostEventChannel playerBoostEventChannel;

        //BEN_CORRECTIOn : Devrait être une variable locale. Aucune raison que ce soit un attribut.
        //
        //                 Je ne veux plus voir ce genre de chose. C'est du niveau 2e session.
        private float fillAmount;

        private void InjectBoostBarController([EventChannelScope] PlayerBoostEventChannel playerBoostEventChannel)
        {
            this.playerBoostEventChannel = playerBoostEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectBoostBarController");
            playerBoostEventChannel.OnEventPublished += OnBoostChanged;
        }

        private void OnDestroy()
        {
            playerBoostEventChannel.OnEventPublished -= OnBoostChanged;
        }

        private void OnBoostChanged(PlayerBoostEvent playerBoostEvent)
        {
            if (barVisual != null)
            {
                boostStats = playerBoostEvent.PlayerBoost;
                fillAmount = boostStats.BoostPoints / boostStats.MaxBoostPoints;
                //BEN_REVIEW : Ah...je viens d'en apprendre une là...
                barVisual.fillAmount = fillAmount;
            }   
        }
    }
}