namespace Harmony
{
    /// <summary>
    /// Représente un champ de texte dans une interface graphique.
    /// </summary>
    public interface ITextInput : ISelectable
    {
        /// <summary>
        /// Texte actuellement dans le champ de texte.
        /// </summary>
        string Text { get; set; }
    }
}