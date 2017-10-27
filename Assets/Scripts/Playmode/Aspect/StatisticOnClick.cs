using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class StatisticOnClick : GameScript
    {
        [Tooltip("Bouton pour aller voir les statistiques.")]
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
