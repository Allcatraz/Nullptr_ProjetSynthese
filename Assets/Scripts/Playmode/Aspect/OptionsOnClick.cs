using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/OptionsOnClick")]
    public class OptionsOnClick : GameScript
    {
        [Tooltip("Button des options.")]
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
           
        }
    }
}
