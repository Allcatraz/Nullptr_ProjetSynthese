using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class BoostBarController : GameScript
    {
        [SerializeField] private Image image;

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
            fillAmount = boostStats.BoostPoints / boostStats.MaxBoostPoints;
            image.fillAmount = fillAmount;
        }
    }
}