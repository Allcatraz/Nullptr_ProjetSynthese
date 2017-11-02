using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/QuitMenuOnClick")]
    public class QuitMenuOnClick : GameScript
    {
        [Tooltip("Button utilisé pour quitter la fenêtre.")]
        [SerializeField]
        private Button button;

        [Tooltip("Fenêtre que le bouton doit faire quitter.")]
        [SerializeField]
        private RectTransform window;

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
            window.gameObject.SetActive(false);
        }
    }
}
