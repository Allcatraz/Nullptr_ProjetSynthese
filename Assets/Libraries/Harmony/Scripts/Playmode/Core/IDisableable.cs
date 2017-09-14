namespace Harmony
{
    /// <summary>
    /// Représente un composant pouvant être activé ou désactivé.
    /// </summary>
    public interface IDisableable : IComponent
    {
        /// <summary>
        /// Active/Désactive ce component. Un component désactivé n'agis plus sur le monde, mais le monde
        /// peut continuer d'agir sur lui (pour, par exemple, le réactiver).
        /// </summary>
        bool Enabled { get; set; }
    }
}