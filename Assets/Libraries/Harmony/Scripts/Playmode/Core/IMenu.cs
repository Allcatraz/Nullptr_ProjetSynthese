namespace Harmony
{
    /// <summary>
    /// Représente un menu. Un menu est une interface graphique.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Plusieurs menus peuvent être affichés les uns sur les autres. Par contre, seul le menu sur le dessus de la pile est 
    /// actif, les autres étant toujours affichés mais inactifs. 
    /// </para>
    /// </remarks>
    /// <seealso cref="IActivity"/>
    /// <seealso cref="IMenuStack"/>
    public interface IMenu : IData
    {
        /// <summary>
        /// Scène du menu. (Obligatoire)
        /// </summary>
        R.E.Scene Scene { get; }

        /// <summary>
        /// Identifiant du GameObject contenant le controleur du menu. (Facultatif)
        /// </summary>
        R.E.GameObject Controller { get; }

        /// <summary>
        /// Indique si le menu est toujours visible.
        /// </summary>
        /// <remarks>
        /// Un menu toujours visible ne peut pas être ajouté sur la pile de menu. Il est par contre toujours 
        /// en dessous des menus empilés sur la pile.
        /// </remarks>
        /// <returns>Vrai si le menu est toujours affiché, faux sinon.</returns>
        bool IsAllwaysVisible();
    }
}