using Harmony;
using UnityEditor;

namespace ProjetSynthese
{
    [CustomEditor(typeof(HitStimulus), true)]
    public class HitStimulusInspector : SensorInspector
    {
        protected override void OnDraw()
        {
            base.OnDraw();
            DrawSensorInspector(typeof(HitStimulus), R.E.Layer.HitSensor);
        }
    }
}