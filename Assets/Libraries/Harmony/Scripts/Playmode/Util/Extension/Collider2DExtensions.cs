using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Signature que tout méthode désirant être notifié du déclanchement d'un trigger 2D doit implémenter.
    /// </summary>
    public delegate void Trigger2DEventHandler(Collider2D other);

    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Renderers.
    /// </summary>
    public static class Collider2DExtensions
    {
        public static Collider2DEventsWrapper Events(this Collider2D collider2D)
        {
            Collider2DEventsWrapper eventsWrapper = collider2D.gameObject.GetComponent<Collider2DEventsWrapper>();
            if (eventsWrapper == null)
            {
                eventsWrapper = collider2D.gameObject.AddComponent<Collider2DEventsWrapper>();
            }
            return eventsWrapper;
        }

        /// <summary>
        /// Wrapper événementiel pour les Collider2D.
        /// </summary>
        public class Collider2DEventsWrapper : Script
        {
            /// <summary>
            /// Évènement se déclanchant lorsqu'un autre trigger entre en collision avec celui-ci.
            /// </summary>
            public event Trigger2DEventHandler OnEnterTrigger;

            /// <summary>
            /// Évènement se déclanchant lorsqu'un autre trigger n'est plus en collision avec celui-ci.
            /// </summary>
            public event Trigger2DEventHandler OnExitTrigger;

            private void OnTriggerEnter2D(Collider2D other)
            {
                if (OnEnterTrigger != null) OnEnterTrigger(other);
            }

            private void OnTriggerExit2D(Collider2D other)
            {
                if (OnExitTrigger != null) OnExitTrigger(other);
            }
        }
    }
}