using Harmony;
using UnityEditor;

namespace ProjetSynthese
{
    [CustomEditor(typeof(ItemStimulus), true)]
    public class ItemStimulusInspector : SensorInspector
    {
        protected override void OnDraw()
        {
            base.OnDraw();
            DrawSensorInspector(typeof(ItemStimulus), R.E.Layer.ItemSensor);
        }
    }
}