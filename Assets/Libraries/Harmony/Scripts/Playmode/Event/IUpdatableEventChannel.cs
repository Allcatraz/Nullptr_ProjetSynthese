using JetBrains.Annotations;

namespace Harmony.EventSystem
{
    /// <summary>
    /// Signature de toute fonction désirant recevoir une mise à jour à partir d'un <see cref="IEventChannel{T}"/>.
    /// </summary>
    public delegate void EventChannelUpdateHandler<in U>([NotNull] U newUpdate) where U : IUpdate;

    /// <summary>
    /// Signature de toute fonction désirant être notifié d'une requête de mise à jour sur un <see cref="IEventChannel{T}"/>.
    /// </summary>
    public delegate void EventChannelUpdateRequestHandler<out U>([NotNull] EventChannelUpdateHandler<U> updateHandler) where U : IUpdate;

    /// <summary>
    /// Canal d'événements spécial où il est possible de requérir une mise à jour.
    /// </summary>
    /// <typeparam name="T">Type d'événement circulant sur le canal.</typeparam>
    /// <typeparam name="U">Type de mise à jour circulant sur le canal.</typeparam>
    public interface IUpdatableEventChannel<T, U> : IEventChannel<T> where T : IEvent where U : IUpdate
    {
        /// <summary>
        /// Évènement déclanché lorsqu'une requête de mise à jour est demandée sur ce canal.
        /// </summary>
        event EventChannelUpdateRequestHandler<U> OnUpdateRequested;

        /// <summary>
        /// Permet de demander une mise à jour sur le Canal. Tous ceux pouvant répondre à la demande seront alors mis à contribution.
        /// </summary>
        /// <param name="updateHandler">
        /// Fonction à appeler pour recevoir les mises à jour.
        /// </param>
        void RequestUpdate([NotNull] EventChannelUpdateHandler<U> updateHandler);
    }
}