using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    //BEN_CORRECTION : Ceci est une view, pas un contrôleur.
    //
    //                 Au sujet du UI, je ne vous empêche pas d'avoir un contrôleur tel qu'un "HudController".
    //                 Ce "HudController" pourrait s'abonner aux différents EventChannels qui publient des informations
    //                 au sujet du joueur (points de vie, distance entre le joueur et le death circle, etc...)
    //                 Ce "HudController" reçoit un évènement, et le redirige à la bonne "view" qui change
    //                 l'interface en fonction de la valeur que l'on lui envoie (comme les points de vie par exemple).
    //
    //                 Au pire, si c'est pas clair, on s'en reparlera.
    public class HealthBarController : GameScript
    {
        [Tooltip("Le visuel de la bar de health.")]
        [SerializeField] private Image barVisual;
        private PlayerHealthEventChannel playerHealthEventChannel;
        private Health health;

        private float fillAmount;

        private void InjectHealthBarController([EventChannelScope] PlayerHealthEventChannel playerHealthEventChannel)
        {
            this.playerHealthEventChannel = playerHealthEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectHealthBarController");
            playerHealthEventChannel.OnEventPublished += OnHealthChanged;
        }

        private void OnDestroy()
        {
            playerHealthEventChannel.OnEventPublished -= OnHealthChanged;
        }

        private void OnHealthChanged(PlayerHealthEvent newEvent)
        {
            health = newEvent.PlayerHealth;
            if (health != null)
            {
                fillAmount = health.HealthPoints / health.MaxHealthPoints;
                barVisual.fillAmount = fillAmount;
            }
        }
    }
}