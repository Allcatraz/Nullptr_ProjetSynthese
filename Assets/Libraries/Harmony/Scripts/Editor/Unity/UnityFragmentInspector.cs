using UnityEditor;

namespace Harmony.Unity
{
    [CustomEditor(typeof(UnityFragment))]
    public class UnityFragmentInspector : UnityInspector
    {
        protected override void OnDraw()
        {
            DrawEnumPropertyGrid(GetEnumProperty("scene", typeof(R.E.Scene)), 2);
            DrawEnumPropertyGrid(GetEnumProperty("controller", typeof(R.E.GameObject)), 2);
        }
    }
}