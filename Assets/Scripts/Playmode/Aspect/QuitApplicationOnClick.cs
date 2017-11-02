using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/QuitApplicationOnClick")]
    public class QuitApplicationOnClick : GameScript
    {
        [Tooltip("Boutton pour quitter l'application.")]
        [SerializeField] private Button button;

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
            Application.Quit();         
        }
    }
}
