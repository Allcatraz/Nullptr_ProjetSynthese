using Harmony.EventSystem;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un EventChannel Unity.
    /// </summary>
    /// <typeparam name="T">Type d'événement circulant sur le canal.</typeparam>
    public abstract class UnityEventChannel<T> : UnityScript, IEventChannel<T> where T : IEvent
    {
        public virtual event EventChannelHandler<T> OnEventPublished;

        public virtual void Publish(T newEvent)
        {
            if (OnEventPublished != null) OnEventPublished(newEvent);
        }
    }
}