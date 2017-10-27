using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class ConnectOnClick : GameScript
    {
        [Tooltip("Button connect.")]
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
