using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
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