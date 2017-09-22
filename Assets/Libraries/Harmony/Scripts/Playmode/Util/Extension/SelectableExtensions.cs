using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Harmony
{
    /// <summary>
    /// Signature que tout méthode désirant être notifié qu'un élément est sélectionné doit implémenter.
    /// </summary>
    public delegate void SelectedEventHandler();

    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Selectable.
    /// </summary>
    public static class SelectableExtensions
    {
        public static SelectableEventWrapper Events(this Selectable selectable)
        {
            SelectableEventWrapper eventsWrapper = selectable.gameObject.GetComponent<SelectableEventWrapper>();
            if (eventsWrapper == null)
            {
                eventsWrapper = selectable.gameObject.AddComponent<SelectableEventWrapper>();
            }
            return eventsWrapper;
        }

        /// <summary>
        /// Retourne l'élément sélectionné actuellement dans l'interface.
        /// </summary>
        /// <returns>L'élément sélectionné actuellement</returns>
        public static Selectable GetCurrentlySelected()
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem != null)
            {
                GameObject selectedGameObject = eventSystem.currentSelectedGameObject;
                if (selectedGameObject != null)
                {
                    Selectable selectable = selectedGameObject.GetComponent<Selectable>();
                    if (selectable != null)
                    {
                        return selectable;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Selectionne l'élément suivant.
        /// </summary>
        /// <param name="selectable">Élément dont le suivant doit être sélectionné.</param>
        /// <returns>Nouvel élément sélectionné.</returns>
        public static Selectable SelectNext(this Selectable selectable)
        {
            Selectable nextSelectable = selectable.navigation.selectOnDown;
            if (nextSelectable != null)
            {
                nextSelectable.Select();
            }
            return nextSelectable;
        }

        /// <summary>
        /// Selectionne l'élément précédent.
        /// </summary>
        /// <param name="selectable">Élément dont le précédent doit être sélectionné.</param>
        /// <returns>Nouvel élément sélectionné.</returns>
        public static Selectable SelectPrevious(this Selectable selectable)
        {
            Selectable previousSelectable = selectable.navigation.selectOnUp;
            if (previousSelectable != null)
            {
                previousSelectable.Select();
            }
            return previousSelectable;
        }

        /// <summary>
        /// Effectue un "Clic".
        /// </summary>
        /// <param name="selectable">Élément sur lequel le clic doit être effectué.</param>
        public static void Click(this Selectable selectable)
        {
            ExecuteEvents.Execute(selectable.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        /// <summary>
        /// Wrapper événementiel pour les Selectable.
        /// </summary>
        public class SelectableEventWrapper : Script, ISelectHandler
        {
            public event SelectedEventHandler OnSelected;

            /// <summary>
            /// Évènement se déclanchant lorsque l'élément est sélectionné.
            /// </summary>
            public void OnSelect(BaseEventData eventData)
            {
                if (OnSelected != null) OnSelected();
            }
        }
    }
}