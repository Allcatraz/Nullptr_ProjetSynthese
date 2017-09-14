using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Menu Unity.
    /// </summary>
    /// <inheritdoc cref="IMenu"/>
    [CreateAssetMenu(fileName = "New Menu", menuName = "Game/Menu")]
    public class UnityMenu : UnityData, IMenu
    {
        [SerializeField]
        private R.E.Scene scene = R.E.Scene.None;

        [SerializeField]
        private R.E.GameObject controller = R.E.GameObject.None;

        [SerializeField]
        private bool isAllwaysVisible = false;

        public R.E.Scene Scene
        {
            get { return scene; }
        }

        public R.E.GameObject Controller
        {
            get { return controller; }
        }

        public bool IsAllwaysVisible()
        {
            return isAllwaysVisible;
        }
    }
}