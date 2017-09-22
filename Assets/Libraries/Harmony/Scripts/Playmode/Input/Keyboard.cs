using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente une entrée de type clavier.
    /// </summary>
    [AddComponentMenu("Game/Input/Keyboard")]
    public class Keyboard : Script
    {
        /// <summary>
        /// Indique si une touche est enfoncée actuellement.
        /// </summary>
        /// <param name="key">Touche à vérifier.</param>
        /// <returns>Vrai si la touche est enfoncée, faux sinon.</returns>
        public bool GetKey(KeyCode key)
        {
            return Input.GetKey(key);
        }

        /// <summary>
        /// Indique si une touche vient d'être enfoncée. Autrement dit, vérifie si son état est passé
        /// à <i>enfoncée</i> depuis le dernier appel à cette méthode pour la même touche.
        /// </summary>
        /// <param name="key">Touche à vérifier.</param>
        /// <returns>Vrai si la touche est désormais enfoncée, faux sinon.</returns>
        public bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }
    }
}