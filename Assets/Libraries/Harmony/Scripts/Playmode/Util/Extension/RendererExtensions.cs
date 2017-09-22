using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Signature que tout méthode désirant être notifié de l'apparition d'un objet à l'écran doit implémenter.
    /// </summary>
    public delegate void RendererVisibleEventHandler();

    /// <summary>
    /// Signature que tout méthode désirant être notifié de la disparition d'un objet à l'écran doit implémenter.
    /// </summary>
    public delegate void RendererInvisibleEventHandler();

    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Renderers.
    /// </summary>
    public static class RendererExtensions
    {
        public static RendererEventsWrapper Events(this Renderer renderer)
        {
            RendererEventsWrapper eventsWrapper = renderer.gameObject.GetComponent<RendererEventsWrapper>();
            if (eventsWrapper == null)
            {
                eventsWrapper = renderer.gameObject.AddComponent<RendererEventsWrapper>();
            }
            return eventsWrapper;
        }

        /// <summary>
        /// Wrapper événementiel pour les Renderer.
        /// </summary>
        public class RendererEventsWrapper : Script
        {
            /// <summary>
            /// Évènement se déclanchant lorsque l'objet apparait à l'écran (même si c'est seulement son ombre).
            /// </summary>
            public event RendererVisibleEventHandler OnIsVisible;

            /// <summary>
            /// Évènement se déclanchant lorsque l'objet n'est plus rendu à l'écran (ni même son ombre).
            /// </summary>
            public event RendererInvisibleEventHandler OnIsInvisible;

            private void OnBecameVisible()
            {
                if (CanFireEventSafely())
                {
                    if (OnIsVisible != null) OnIsVisible();
                }
            }

            private void OnBecameInvisible()
            {
                if (CanFireEventSafely())
                {
                    if (OnIsInvisible != null) OnIsInvisible();
                }
            }

            //This is used prevent event from firing when the scene is stoped in the editor or when the object is destroyed.
            private bool CanFireEventSafely()
            {
                return gameObject.activeSelf &&
                       isActiveAndEnabled &&
                       transform != null
#if UNITY_EDITOR
                       && UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode
#endif
                    ;
            }
        }
    }
}