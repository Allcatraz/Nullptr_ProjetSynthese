using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class HealthBarController : GameScript
    {
        [SerializeField]
        private Image image;
        private PlayerHealthEventChannel playerHealthEventChannel;
        private Health health;

        private float fillAmount;

        private void Update()
        {
            SetFillAmountFromHealth();
            UpdateBar();
        }

        private void InjectHealthBarController([EventChannelScope] PlayerHealthEventChannel playerHealthEventChannel)
        {
            this.playerHealthEventChannel = playerHealthEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectHealthBarController");
            playerHealthEventChannel.OnEventPublished += PlayerHealthEventChannel_OnEventPublished;
        }

        private void PlayerHealthEventChannel_OnEventPublished(PlayerHealthEvent newEvent)
        {
            this.health = newEvent.PlayerHealth;
        }

        private void SetFillAmountFromHealth()
        {
            if (health != null)
            {
                fillAmount = (float)health.HealthPoints / health.MaxHealthPoints;
            }
        }

        private void UpdateBar()
        {
            image.fillAmount = fillAmount;
        }
    }
}

