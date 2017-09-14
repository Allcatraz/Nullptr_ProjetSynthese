namespace Harmony
{
    /// <summary>
    /// Représente une chaine de caractères dans une interface graphique.
    /// </summary>
    public interface ITextView : IDisableable
    {
        /// <summary>
        /// Chaine de caractères affichée par ce TextView.
        /// </summary>
        string Text { get; set; }
    }
}