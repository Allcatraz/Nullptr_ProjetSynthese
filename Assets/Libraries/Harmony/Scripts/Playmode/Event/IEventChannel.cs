using JetBrains.Annotations;

namespace Harmony.EventSystem
{
    /// <summary>
    /// Signature de toute fonction désirant être recevoir un un évènement publié sur un <see cref="IEventChannel{T}"/>.
    /// </summary>
    public delegate void EventChannelHandler<in T>([NotNull] T newEvent) where T : IEvent;

    /// <summary>
    /// Base de tout canal d'événements.
    /// </summary>
    /// <typeparam name="T">Type d'événement circulant sur le canal.</typeparam>
    public interface IEventChannel<T> where T : IEvent
    {
        /// <summary>
        /// Évènement déclanché lorsqu'un <see cref="IEvent"/> est publié sur ce canal.
        /// </summary>
        event EventChannelHandler<T> OnEventPublished;

        /// <summary>
        /// Publie un évènement sur le canal. Tout ceux enregistré auprès du canal recevront cet événement.
        /// </summary>
        /// <param name="newEvent"><see cref="IEvent"/> à publier sur le canal.</param>
        void Publish([NotNull] T newEvent);
    }
}