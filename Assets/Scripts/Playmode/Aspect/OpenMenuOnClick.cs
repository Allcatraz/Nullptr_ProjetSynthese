using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/OpenMenuOnClick")]
    public class OpenMenuOnClick : GameScript
    {
        [Tooltip("Button utilisé pour ouvrir la fenêtre de création d'un compte.")]
        [SerializeField]
        private Button button;

        [Tooltip("Fenêtre qui sera ouverte lors du clic du bouton.")]
        [SerializeField]
        private RectTransform createAccountWindow;

        private void OnEnable()
        {
            button.Events().OnClick += OnClick;
        }

        private void OnDisable()
        {
            button.Events().OnClick -= OnClick;
        }

        private void OnClick()
        {
            createAccountWindow.gameObject.SetActive(true);
        }
    }
}
