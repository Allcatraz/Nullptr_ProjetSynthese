using Harmony;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjetSynthese
{
    public class HouseController : GameScript
    {
        [Tooltip("Le matériel pour l'aspet du toit.")]
        [SerializeField] private Material roofMaterial;

        private PlayerChangeModeEventChannel playerChangeModeEventChannel;

        private void InjectHouseController([EventChannelScope] PlayerChangeModeEventChannel playerChangeModeEventChannel)
        {
            this.playerChangeModeEventChannel = playerChangeModeEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectHouseController");
            playerChangeModeEventChannel.OnEventPublished += OnPlayerChangeMode;
        }

        private void OnDestroy()
        {
            playerChangeModeEventChannel.OnEventPublished -= OnPlayerChangeMode;
        }

        private void OnPlayerChangeMode(PlayerChangeModeEvent playerChangeModeEvent)
        {
            if (!playerChangeModeEvent.IsPlayerInFirstPerson)
            {
                roofMaterial.SetInt("_SrcBlend", (int)BlendMode.One);
                roofMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                roofMaterial.SetInt("_ZWrite", 0);
                roofMaterial.DisableKeyword("_ALPHATEST_ON");
                roofMaterial.DisableKeyword("_ALPHABLEND_ON");
                roofMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                roofMaterial.renderQueue = 3000;
            }
            else
            {
                roofMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                roofMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                roofMaterial.SetInt("_ZWrite", 1);
                roofMaterial.DisableKeyword("_ALPHATEST_ON");
                roofMaterial.DisableKeyword("_ALPHABLEND_ON");
                roofMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                roofMaterial.renderQueue = -1;
            }
        }
    }
}
