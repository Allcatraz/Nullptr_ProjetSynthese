namespace Harmony
{
    /// <summary>
    /// Représente l'état global des menus de l'application.
    /// </summary>
    public interface IMenuState
    {
        /// <summary>
        /// Élément actuellement sélectionné (possédant le "focus") par l'utilisateur dans le menu actuel.
        /// </summary>
        ISelectable CurrentSelected { get; }
    }
}