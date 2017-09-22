namespace Harmony
{
    /// <summary>
    /// Représente un évènement circulant sur un <see cref="EventChannel{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Un jeu vidéo est en général un amas complexe d'événements en tout genre lancé partout au travers
    /// de l'application. La gestion de ces événements est d'ailleurs l'un des plus gros défis du développement
    /// d'un jeu vidéo.
    /// </para>
    /// <para>
    /// Un des patrons de conception populaire pour gérer cela est le Canal, ou Channel en anglais. Un Channel
    /// est tel un flux d'événements sur lequel tout le monde peut publier des informations et où tout le monde
    /// peut s'enregistrer pour être notifié d'évènements.
    /// </para>
    /// <para>
    /// En général, un jeu possède un bon nombre de Channels, et les éléments du jeu s'abonnent tout simplement à ce
    /// qui les intéressent. Comme le flux peut être assez conséquent, il est conseillé de subdiviser tout Channel
    /// très occupé en plusieurs petits Channel. Notez qu'il est de la responsabilité des éléments du jeu de publier 
    /// sur les Channels. En général, on ajoute un nouveau composant sur le GameObject juste pour cela.
    /// </para>
    /// <para>
    /// Un Channel est en fait composé de 3 parties : le type d'évènement (voir IEvent), un canal (voir EventChannel)
    /// et un « Publisher ».
    /// </para>
    /// <para>
    /// Un Event doit contenir suffisamment d'informations pour représenter ce qui s'est produit. N'hésitez pas à en
    /// mettre plus que pas assez. Par exemple :
    /// </para>
    /// <code>
    /// public class ActorHealthChangedEvent : IEvent
    /// {
    ///     public Health Health { get; private set; }
    ///     public int OldHealthPoints { get; private set; }
    ///     public int NewHealthPoints { get; private set; }
    /// 
    ///     public ActorHealthChangedEvent(Health health, int oldHealthPoints, int newHealthPoints)
    ///     {
    ///         Health = health;
    ///         OldHealthPoints = oldHealthPoints;
    ///         NewHealthPoints = newHealthPoints;
    ///     }
    /// }
    /// </code>
    /// Un EventChannel est beaucoup plus simple à créer : il suffit d'hériter EventChannel. Par exemple :
    /// <code>
    /// public class ActorHealthChangedEventChannel : EventChannel<ActorHealthChangedEvent>
    /// {
    /// }
    /// </code>
    /// <para>
    /// Le dernier est un EventPublisher. Ce type de component sert spécifiquement à créer des Event et à les envoyer sur
    /// un EventChannel. Par exemple :
    /// </para>
    /// <code>
    /// public class ActorHealthChangedEventPublisher : Script
    /// {
    ///     private Health health;
    ///     private ActorHealthChangedEventChannel eventChannel;
    ///     
    ///     public void Awake()
    ///     {
    ///         //Get Health component
    ///         //Get ActorHealthChangedEventChannel component
    ///     }
    ///     
    ///     public void OnEnable()
    ///     {
    ///         health.OnHealthChanged += OnHealthChanged;
    ///     }
    ///     
    ///     public void OnDisable()
    ///     {
    ///         health.OnHealthChanged -= OnHealthChanged;
    ///     }
    ///     
    ///     private void OnHealthChanged(int oldHealthPoints, int newHealthPoints)
    ///     {
    ///         eventChannel.Publish(new ActorHealthChangedEvent(health, oldHealthPoints, newHealthPoints));
    ///     }
    /// }
    /// </code>
    /// <para>
    /// Il ne reste plus qu'à s'enregistrer auprès d'un EventChannel comme ceci :
    /// </para>
    /// <code>
    /// public class GameController : Script
    /// {
    ///     private ActorHealthChangedEventChannel eventChannel;
    ///     
    ///     public void Awake()
    ///     {
    ///         //Get ActorHealthChangedEventChannel component
    ///     }
    ///     
    ///     public void OnEnable()
    ///     {
    ///         eventChannel.OnEventPublished += OnActorHealthChanged;
    ///     }
    ///     
    ///     public void OnDisable()
    ///     {
    ///         eventChannel.OnEventPublished -= OnActorHealthChanged;
    ///     }
    ///     
    ///     private void OnActorHealthChanged(ActorHealthChangedEvent healthEvent)
    ///     {
    ///         //Do something
    ///     }
    /// }
    /// </code>
    /// </remarks>
    public interface IEvent
    {
    }
}