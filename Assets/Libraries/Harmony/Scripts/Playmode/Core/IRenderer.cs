namespace Harmony
{
    /// <summary>
    /// Signature de toute fonction désirant être notifié que le GameObject devient visble à l'écran.
    /// </summary>
    public delegate void RendererVisibleEventHandler();

    /// <summary>
    /// Signature de toute fonction désirant être notifié que le GameObject n'est plus visble à l'écran.
    /// </summary>
    public delegate void RendererInvisibleEventHandler();

    /// <summary>
    /// Représente un Renderer, permettant de rendre un GameObject visible à l'écran.
    /// </summary>
    public interface IRenderer : IComponent
    {
        /// <summary>
        /// Évènement se produisant lorsque le GameObject devient visible à l'écran, c'est-à-dire
        /// dans au moins une caméra ou que son ombrage, reflet ou tout autre effet spécial devient visible.
        /// </summary>
        event RendererVisibleEventHandler OnBecameVisible;

        /// <summary>
        /// Évènement se produisant lorsque le GameObject n'est plus visible à l'écran d'aucune façon, c'est-à-dire
        /// dans aucune caméra et que son ombrage, reflet ou tout autre effet spécial n'est aussi plus visible.
        /// </summary>
        event RendererInvisibleEventHandler OnBecameInvisible;

        /// <summary>
        /// Indique si, oui ou non, le GameObject est visible à l'écran.
        /// </summary>
        /// <returns>Vrai si le GameObject est visible à l'écran, faux sinon.</returns>
        bool IsVisible();
    }
}