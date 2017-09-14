using Harmony.EventSystem;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente une canal d'événements spécial où il est possible de requérir une mise à jour pour Unity.
    /// </summary>
    /// <typeparam name="T">Type d'événement circulant sur le canal.</typeparam>
    /// <typeparam name="U">Type de mise à jour circulant sur le canal.</typeparam>
    public abstract class UnityUpdatableEventChannel<T, U> : UnityEventChannel<T>, IUpdatableEventChannel<T, U> where T : IEvent where U : IUpdate
    {
        public virtual event EventChannelUpdateRequestHandler<U> OnUpdateRequested;

        public virtual void RequestUpdate(EventChannelUpdateHandler<U> updateHandler)
        {
            if (OnUpdateRequested != null) OnUpdateRequested(updateHandler);
        }
    }
}