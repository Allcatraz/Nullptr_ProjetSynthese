using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente une Activité. Une activité est un lot de <see cref="Fragment">Fragments</see> et de <see cref="Menu">Menus</see>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Une seule activité est affichée à la fois. Si une autre activité est démarrée, l'activité courante est déchargée 
    /// au profit de la nouvelle activité. La nouvelle activité est ensuite mise sur le dessus de la pile d'activités en cours. 
    /// </para>
    /// <para>
    /// Lorsque l'activité courante est arrêtée, cette dernière est déchargée, retirée de la pile, et c'est la nouvelle 
    /// activité sur le dessus de la pile qui est chargée.
    /// </para>
    /// <para>
    /// S'il n'y a plus aucune activité sur la pile, mais que l'on demande tout de même d'arrêter l'activité courante,
    /// alors c'est l'application au complet qui est arrêtée.
    /// </para>
    /// <para>
    /// Pour charger des Activités, utilisez un IActivityStack.
    /// </para>
    /// </remarks>
    /// <seealso cref="Fragment"/>
    /// <seealso cref="Menu"/>
    /// <seealso cref="ActivityStack"/>
    [CreateAssetMenu(fileName = "New Activity", menuName = "Game/Activities/Activity")]
    public class Activity : ScriptableObject
    {
        [SerializeField]
        private R.E.Scene scene = R.E.Scene.None;

        [SerializeField]
        private R.E.GameObject controller = R.E.GameObject.None;

        [SerializeField]
        private Fragment[] fragments = new Fragment[0];

        [SerializeField]
        private Menu[] menus = new Menu[0];

        [SerializeField]
        private Fragment activeFragmentOnLoad = null;

        /// <summary>
        /// Scène de l'activité. (Facultatif)
        /// </summary>
        public R.E.Scene Scene
        {
            get { return scene; }
        }

        /// <summary>
        /// Identifiant du GameObject contenant le controleur de l'activité. (Facultatif)
        /// </summary>
        public R.E.GameObject Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Lot de fragments de l'activité. (Facultatif) 
        /// </summary>
        public IList<Fragment> Fragments
        {
            get { return fragments; }
        }

        /// <summary>
        /// Lot de menus de l'activité. (Facultatif)
        /// </summary>
        public IList<Menu> Menus
        {
            get { return menus; }
        }

        /// <summary>
        /// Fragment à marquer comme actif lors du chargement de l'activité (Facultatif).
        /// </summary>
        public Fragment ActiveFragmentOnLoad
        {
            get { return activeFragmentOnLoad; }
        }
    }
}