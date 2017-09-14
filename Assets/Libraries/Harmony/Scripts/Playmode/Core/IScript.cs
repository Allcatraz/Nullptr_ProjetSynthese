#pragma warning disable 1587
/// <summary>
/// Harmony est composée de plusieurs modules, tous ayant une responsabilité précise visant à simplifier ou améliorer le 
/// développement de projets de jeu-vidéo.
/// </summary>
#pragma warning restore 1587
namespace Harmony
{
    /// <summary>
    /// Représente un script au sein d'un jeu. Est toujours associé à un GameObject.
    /// </summary>
    /// <remarks>
    /// Le IScript ajoute plusieurs moyens d'obtenir des <i>Components</i> ou des <i>GameObjects</i>. Par exemple, 
    /// il est désormais possible d'obtenir tous les enfants d'un <i>GameObject</i>. Consultez les différentes méthodes pour les détails.
    /// </remarks>
    public interface IScript
    {
    }
}