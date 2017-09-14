using Harmony.Testing;
using UnityEngine.UI;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un TextView Unity.
    /// </summary>
    /// <inheritdoc cref="ITextView"/>
    [NotTested(Reason.Wrapper)]
    public class UnityTextView : UnityDisableable, ITextView
    {
        private readonly Text text;

        public UnityTextView(Text text) : base(text)
        {
            this.text = text;
        }

        public string Text
        {
            get { return text.text; }
            set { text.text = value; }
        }
    }
}