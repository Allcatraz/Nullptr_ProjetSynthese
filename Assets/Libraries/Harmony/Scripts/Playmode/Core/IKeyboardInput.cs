using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente une entrée de type clavier.
    /// </summary>
    public interface IKeyboardInput
    {
        /// <summary>
        /// Indique si une touche est enfoncée actuellement.
        /// </summary>
        /// <param name="key">Touche à vérifier.</param>
        /// <returns>Vrai si la touche est enfoncée, faux sinon.</returns>
        bool GetKey(KeyCode key);

        /// <summary>
        /// Indique si une touche vient d'être enfoncée. Autrement dit, vérifie si son état est passé
        /// à <i>enfoncée</i> depuis le dernier appel à cette méthode pour la même touche.
        /// </summary>
        /// <param name="key">Touche à vérifier.</param>
        /// <returns>Vrai si la touche est désormais enfoncée, faux sinon.</returns>
        bool GetKeyDown(KeyCode key);
    }
}