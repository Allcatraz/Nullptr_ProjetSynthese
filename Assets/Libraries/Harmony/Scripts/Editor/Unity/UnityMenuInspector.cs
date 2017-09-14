using UnityEditor;

namespace Harmony.Unity
{
    [CustomEditor(typeof(UnityMenu))]
    public class UnityMenuInspector : UnityInspector
    {
        protected override void OnDraw()
        {
            DrawEnumPropertyGrid(GetEnumProperty("scene", typeof(R.E.Scene)), 2);
            DrawEnumPropertyGrid(GetEnumProperty("controller", typeof(R.E.GameObject)), 2);
            DrawTitleLabel("Is Menu Allways Visible on Screen?");
            DrawBasicProperty(GetBasicProperty("isAllwaysVisible"));
        }
    }
}