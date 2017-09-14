using Harmony.Unity;

#pragma warning disable 1587
/// <summary>
/// Module de Harmony servant à créer facilement des évènements globaux au sein du jeu.
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
/// Un Channel est en fait composé de 3 parties : le type d'évènement (voir IEvent), un canal (voir IEventChannel)
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
/// Un EventChannel est beaucoup plus simple à créer : il suffit d'hériter d'une implémentation
/// de IEventChannel, tel que UnityEventChannel. Par exemple :
/// <code>
/// public class ActorHealthChangedEventChannel : UnityEventChannel<ActorHealthChangedEvent>
/// {
/// }
/// </code>
/// <para>
/// Le dernier est un EventPublisher. Ce type de component sert spécifiquement à créer des Event et à les envoyer sur
/// un EventChannel. Par exemple :
/// </para>
/// <code>
/// public class ActorHealthChangedEventPublisher : UnityScript
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
/// public class GameController : UnityScript
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
/// <para>
/// Parfois, certains composants peuvent avoir besoin d'informations à un moment précis, sans qu'aucun événement
/// n'aie encore été publié. Ces composants peuvent alors demander une mise à jour sur un canal.
/// </para>
/// <para>
/// Pour que ce soit possible, il faut tout d'abord créer une structure de données représentant la mise à jour. Par exemple :
/// </para>
/// <code>
/// public class ActorHealthUpdate : IUpdate
/// {
///     public Health Health { get; private set; }
/// 
///     public ActorHealthChangedEvent(Health health)
///     {
///         Health = health;
///     }
/// }
/// </code>
/// <para>
/// Ensuite, pour que le canal soit en mesure de fournir des mises à jour, il faut que ce dernier hérite d'une implémentation
/// de IUpdatableEventChannel, tel que UnityUpdatableEventChannel, au lieu de IEventChannel. Par exemple :
/// </para>
/// <code>
/// public class ActorHealthChangedEventChannel : UnityUpdatableEventChannel<ActorHealthChangedEvent, ActorHealthUpdate>
/// {
/// }
/// </code>
/// <para>
/// Enfin, le EventPublisher doit s'abonner à l'évènement OnUpdateRequested. Par exemple :
/// </para>
/// <code>
/// public class ActorHealthEventPublisher : UnityScript
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
///         eventChannel.OnUpdateRequested += OnRequestUpdate;
///     }
///     
///     public void OnDisable()
///     {
///         health.OnHealthChanged -= OnHealthChanged;
///         eventChannel.OnUpdateRequested -= OnRequestUpdate;
///     }
///     
///     public void OnRequestUpdate(EventChannelUpdateHandler<ActorHealthUpdate> updateHandler)
///     {
///         updateHandler(new ActorHealthUpdate(health));
///     }
///     
///     private void OnHealthChanged(int oldHealthPoints, int newHealthPoints)
///     {
///         eventChannel.Publish(new ActorHealthChangedEvent(health, oldHealthPoints, newHealthPoints));
///     }
/// }
/// </code>
/// <para>
/// Pour effectuer une requête de mise à jour, il suffit d'appeler la méthode RequestUpdate sur le EventChannel
/// en lui fournissant la fonction à appeler. Par exemple :
/// </para>
/// <code>
/// public class GameController : UnityScript
/// {
///     private ActorHealthChangedEventChannel eventChannel;
///     
///     public void Awake()
///     {
///         //Get ActorHealthChangedEvent component
///     }
///     
///     public void OnEnable()
///     {
///         eventChannel.OnEventPublished += OnActorHealthChanged;
///         eventChannel.RequestUpdate(OnActorHealthUpdate);
///     }
///     
///     public void OnDisable()
///     {
///         eventChannel.OnEventPublished -= OnActorHealthChanged;
///     }
///     
///     private void OnActorHealthUpdate(ActorHealthUpdate healthUpdate)
///     {
///         //Do something
///     }
/// 
///     private void OnActorHealthChanged(ActorHealthChangedEvent healthEvent)
///     {
///         //Do something
///     }
/// }
/// </code>
/// </remarks>
#pragma warning restore 1587
namespace Harmony.EventSystem
{
    /// <summary>
    /// Représente un évènement circulant sur un <see cref="UnityEventChannel{T}"/>.
    /// </summary>
    public interface IEvent
    {
    }
}