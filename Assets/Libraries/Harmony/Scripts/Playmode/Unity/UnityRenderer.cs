using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Renderer Unity.
    /// </summary>
    /// <inheritdoc cref="IRenderer"/>
    [NotTested(Reason.Wrapper)]
    public class UnityRenderer : UnityComponent, IRenderer
    {
        private readonly Renderer renderer;
        private UnityRendererEventWrapper eventWrapper;

        public UnityRenderer(Renderer renderer) : base(renderer)
        {
            this.renderer = renderer;
        }

        private UnityRendererEventWrapper EventWrapper
        {
            get
            {
                //Try getting it first, then create it if it doesnt exist.
                if (eventWrapper == null)
                {
                    eventWrapper = renderer.gameObject.GetComponent<UnityRendererEventWrapper>();
                }
                if (eventWrapper == null)
                {
                    eventWrapper = renderer.gameObject.AddComponent<UnityRendererEventWrapper>();
                }
                return eventWrapper;
            }
        }

        public event RendererVisibleEventHandler OnBecameVisible
        {
            add { EventWrapper.OnBecameVisibleEvent += value; }
            remove { EventWrapper.OnBecameVisibleEvent -= value; }
        }

        public event RendererInvisibleEventHandler OnBecameInvisible
        {
            add { EventWrapper.OnBecameInvisibleEvent += value; }
            remove { EventWrapper.OnBecameInvisibleEvent -= value; }
        }

        public bool IsVisible()
        {
            return renderer.isVisible;
        }

        public class UnityRendererEventWrapper : UnityScript
        {
            public event RendererVisibleEventHandler OnBecameVisibleEvent;
            public event RendererInvisibleEventHandler OnBecameInvisibleEvent;

            private void OnBecameVisible()
            {
                if (CanFireEventSafely())
                {
                    if (OnBecameVisibleEvent != null) OnBecameVisibleEvent();
                }
            }

            public void OnBecameInvisible()
            {
                if (CanFireEventSafely())
                {
                    if (OnBecameInvisibleEvent != null) OnBecameInvisibleEvent();
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