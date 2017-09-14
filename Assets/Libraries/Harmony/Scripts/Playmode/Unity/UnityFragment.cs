using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un fragment Unity.
    /// </summary>
    /// <inheritdoc cref="IFragment"/>
    [CreateAssetMenu(fileName = "New Fragment", menuName = "Game/Fragment")]
    public class UnityFragment : UnityData, IFragment
    {
        [SerializeField]
        private R.E.Scene scene = R.E.Scene.None;

        [SerializeField]
        private R.E.GameObject controller = R.E.GameObject.None;

        public R.E.Scene Scene
        {
            get { return scene; }
        }

        public R.E.GameObject Controller
        {
            get { return controller; }
        }
    }
}