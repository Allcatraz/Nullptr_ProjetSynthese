using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class OptionsOnClick : MonoBehaviour
    {
        [Tooltip("Button des options.")]
        [SerializeField] private Button button;
        
        private void OnEnable()
        {
            button.Events().OnClick += OnClick;
        }
        
        private void OnDisable()
        {
            button.Events().OnClick += OnClick;
        }

        private void OnClick()
        {
           
        }
    }
}
