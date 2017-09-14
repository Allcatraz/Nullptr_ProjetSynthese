using Harmony.Testing;
using UnityEngine.UI;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un champ de texte Unity.
    /// </summary>
    /// <inheritdoc cref="IButton"/>
    [NotTested(Reason.Wrapper)]
    public class UnityTextInput : UnitySelectable, ITextInput
    {
        private readonly InputField inputField;

        public UnityTextInput(InputField inputField) : base(inputField)
        {
            this.inputField = inputField;
        }

        public string Text
        {
            get { return inputField.text; }
            set { inputField.text = value; }
        }
    }
}