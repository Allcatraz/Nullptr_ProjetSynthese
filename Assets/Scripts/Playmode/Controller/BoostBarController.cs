using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class BoostBarController : GameScript
    {
        [Tooltip("Visuel de la bar de boost.")]
        [SerializeField] private Image barVisual;

        private BoostStats boostStats;
        private PlayerBoostEventChannel playerBoostEventChannel;

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
                barVisual.fillAmount = fillAmount;
            }   
        }
    }
}