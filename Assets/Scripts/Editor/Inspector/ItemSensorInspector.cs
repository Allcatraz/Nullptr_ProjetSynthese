using Harmony;
using UnityEditor;

namespace ProjetSynthese
{
    [CustomEditor(typeof(ItemSensor), true)]
    public class ItemSensorInspector : SensorInspector
    {
        protected override void OnDraw()
        {
            base.OnDraw();
            DrawSensorInspector(typeof(ItemSensor), R.E.Layer.ItemSensor);
        }
    }
}