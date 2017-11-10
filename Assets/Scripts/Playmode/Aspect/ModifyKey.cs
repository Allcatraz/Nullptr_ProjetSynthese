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

        [Tooltip("Entier représentant la fonction qu'à la key.")]
        [SerializeField]
        private int fonction;

        private KeyCode[] keyCodes = (KeyCode[]) System.Enum.GetValues(typeof(KeyCode));
        private bool canCheckKey = false;
        private Text buttonText;

        private void Awake()
        {
            buttonText = button.GetComponentInChildren<Text>();
        }

        private void OnEnable()
        {
            button.Events().OnClick += OnClick;
            buttonText.text = ActionKey.Instance.GetKeyAt(fonction).ToString();
        }

        private void OnDisable()
        {
            button.Events().OnClick -= OnClick;
        }

        private void FixedUpdate()
        {
            if (canCheckKey)
            {
                for (int i = 0; i < keyCodes.Length; i++)
                {
                    if (Input.GetKeyDown(keyCodes[i]))
                    {
                        buttonText.text = keyCodes[i].ToString();
                        rectTransform.gameObject.SetActive(false);
                        ActionKey.Instance.SetKeyAt(fonction, keyCodes[i]);
                        canCheckKey = false;
                        break;
                    }
                }
            }
        }

        private void OnClick()
        {
            rectTransform.gameObject.SetActive(true);
            canCheckKey = true;
        }
    }
}