using UnityEngine.Events;
using UnityEngine.UI;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Buttons.
    /// </summary>
    public static class ButtonExtensions
    {
        public static ButtonEventsWrapper Events(this Button button)
        {
            return new ButtonEventsWrapper(button);
        }

        /// <summary>
        /// Wrapper événementiel pour les Buttons.
        /// </summary>
        public class ButtonEventsWrapper
        {
            private readonly Button button;

            public ButtonEventsWrapper(Button button)
            {
                this.button = button;
            }

            /// <summary>
            /// Évènement se déclanchant lorsque le bouton est clické.
            /// </summary>
            public event UnityAction OnClick
            {
                add
                {
                    button.onClick.AddListener(value);
                }
                remove
                {
                    button.onClick.RemoveListener(value);
                }
            }
        }
    }
}