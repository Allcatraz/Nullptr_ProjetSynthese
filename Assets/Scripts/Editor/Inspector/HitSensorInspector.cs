using Harmony;
using UnityEditor;

namespace ProjetSynthese
{
    [CustomEditor(typeof(HitSensor), true)]
    public class HitSensorInspector : SensorInspector
    {
        protected override void OnDraw()
        {
            base.OnDraw();
            DrawSensorInspector(typeof(HitSensor), R.E.Layer.HitSensor);
        }
    }
}