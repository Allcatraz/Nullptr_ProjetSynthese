using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Signature de toute fonction désirant savoir lorsqu'un élément d'interface graphique devient sélectioné.
    /// </summary>
    public delegate void SelectedEventHandler();

    /// <summary>
    /// Représente un élément graphique pouvant être sélectionné.
    /// </summary>
    public interface ISelectable : IDisableable
    {
        /// <summary>
        /// Évènement déclanché lorsque cet élément graphique devient sélectionné.
        /// </summary>
        event SelectedEventHandler OnSelected;

        /// <summary>
        /// Simule un clic sur cet élément graphique.
        /// </summary>
        void Click();

        /// <summary>
        /// Marque cet élément graphique comme sélectionné.
        /// </summary>
        void Select();

        /// <summary>
        /// Marque cet élément graphique comme non-sélectionné.
        /// </summary>
        void Deselect();

        /// <summary>
        /// Désélectionne cet élément graphique et sélectionne le prochain, s'il existe.
        /// </summary>
        /// <returns>Le nouvel élément sélectionné, ou null s'il y en a aucun.</returns>
        [CanBeNull]
        ISelectable SelectNext();

        /// <summary>
        /// Désélectionne cet élément graphique et sélectionne le précédent, s'il existe.
        /// </summary>
        /// <returns>Le nouvel élément sélectionné, ou null s'il y en a aucun.</returns>
        [CanBeNull]
        ISelectable SelectPrevious();
    }
}