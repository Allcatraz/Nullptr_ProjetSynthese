using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 1587
/// <summary>
/// Module de Harmony fournissant une implémentation pour Unity.
/// </summary>
#pragma warning restore 1587
namespace Harmony.Unity
{
    /// <summary>
    /// Représente une Activité Unity.
    /// </summary>
    /// <inheritdoc cref="IActivity"/>
    [CreateAssetMenu(fileName = "New Activity", menuName = "Game/Activity")]
    public class UnityActivity : UnityData, IActivity
    {
        [SerializeField]
        private R.E.Scene scene = R.E.Scene.None;

        [SerializeField]
        private R.E.GameObject controller = R.E.GameObject.None;

        [SerializeField]
        private UnityFragment[] fragments = new UnityFragment[0];

        [SerializeField]
        private UnityMenu[] menus = new UnityMenu[0];

        [SerializeField]
        private UnityFragment activeFragmentOnLoad = null;

        public R.E.Scene Scene
        {
            get { return scene; }
        }

        public R.E.GameObject Controller
        {
            get { return controller; }
        }

        public IList<IFragment> Fragments
        {
            get { return fragments; }
        }

        public IList<IMenu> Menus
        {
            get { return menus; }
        }

        public IFragment ActiveFragmentOnLoad
        {
            get { return activeFragmentOnLoad; }
        }
    }
}