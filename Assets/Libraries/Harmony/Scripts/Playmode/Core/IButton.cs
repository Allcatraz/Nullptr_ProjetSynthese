namespace Harmony
{
    /// <summary>
    /// Signature de toute fonction désirant être notifié d'un clic de <see cref="IButton"/>.
    /// </summary>
    public delegate void ButtonClickedEventHandler();

    /// <summary>
    /// Représente un bouton dans une interface graphique.
    /// </summary>
    public interface IButton : ISelectable
    {
        /// <summary>
        /// Évènement déclanché lorsque le bouton est cliqué.
        /// </summary>
        event ButtonClickedEventHandler OnClicked;
    }
}