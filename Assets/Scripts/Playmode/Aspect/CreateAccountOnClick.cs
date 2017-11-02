using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/CreateAccountOnClick")]
    public class CreateAccountOnClick : MonoBehaviour
    {
        [Tooltip("Bouton qui crée l'account du joueur lorsque clické.")]
        [SerializeField]
        private Button button;

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
            
        }
    }
}
