using UnityEditor;

namespace Harmony.Unity
{
    /// <summary>
    /// Inspecteur pour les Menus dans l'éditeur Unity.
    /// </summary>
    [CustomEditor(typeof(Menu))]
    public class UnityMenuInspector : UnityInspector
    {
        private EnumProperty scene;
        private EnumProperty controller;
        private SerializedProperty isAlwaysVisible;

        private void Awake()
        {
            scene = GetEnumProperty("scene", typeof(R.E.Scene));
            controller = GetEnumProperty("controller", typeof(R.E.GameObject));
            isAlwaysVisible = GetBasicProperty("isAlwaysVisible");
        }

        private void OnDestroy()
        {
            scene = null;
            controller = null;
            isAlwaysVisible = null;
        }

        protected override void OnDraw()
        {
            DrawEnumPropertyGrid(scene, 2);
            DrawEnumPropertyGrid(controller, 2);
            DrawTitleLabel("Is Menu Always Visible on Screen?");
            DrawBasicProperty(isAlwaysVisible);
        }
    }
}