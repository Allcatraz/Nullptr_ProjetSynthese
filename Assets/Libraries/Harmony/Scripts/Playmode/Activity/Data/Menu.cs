using UnityEngine;

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
    /// <seealso cref="Activity"/>
    [CreateAssetMenu(fileName = "New Menu", menuName = "Game/Activities/Menu")]
    public class Menu : ScriptableObject
    {
        [SerializeField]
        private R.E.Scene scene = R.E.Scene.None;

        [SerializeField]
        private R.E.GameObject controller = R.E.GameObject.None;

        [SerializeField]
        private bool isAlwaysVisible = false;

        /// <summary>
        /// Scène du menu. (Obligatoire)
        /// </summary>
        public R.E.Scene Scene
        {
            get { return scene; }
        }

        /// <summary>
        /// Identifiant du GameObject contenant le controleur du menu. (Facultatif)
        /// </summary>
        public R.E.GameObject Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Indique si le menu est toujours visible.
        /// </summary>
        /// <remarks>
        /// Un menu toujours visible ne peut pas être ajouté sur la pile de menu. Il est par contre toujours 
        /// en dessous des menus empilés sur la pile.
        /// </remarks>
        /// <returns>Vrai si le menu est toujours affiché, faux sinon.</returns>
        public bool IsAlwaysVisible()
        {
            return isAlwaysVisible;
        }
    }
}