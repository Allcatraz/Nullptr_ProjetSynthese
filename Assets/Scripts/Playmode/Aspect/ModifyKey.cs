using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class ModifyKey : MonoBehaviour
    {
        [Tooltip("Bouton sur lequel l'utilisateur doit appuyer pour modifier la touche.")]
        [SerializeField]
        private Button button;

        [Tooltip("Menu d'avertissement pour l'utilisateur qu'il doit rentrer une touche.")]
        [SerializeField]
        private RectTransform rectTransform;

        private bool canCheckKey = false;

        private void OnEnable()
        {
            button.Events().OnClick += OnClick;
        }

        private void OnDisable()
        {
            button.Events().OnClick -= OnClick;
        }

        private void FixedUpdate()
        {
            if (Input.anyKeyDown && canCheckKey)
            {
                button.GetComponentInChildren<Text>().text = Input.inputString;
                rectTransform.gameObject.SetActive(false);
                canCheckKey = false;
            }
        }

        private void OnClick()
        {
            rectTransform.gameObject.SetActive(true);
            canCheckKey = true;
        }
    }
}
