using System;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public abstract class SensorInspector : GameScriptInspector
    {
        protected void DrawSensorInspector(Type sensorType, R.E.Layer sensorLayer)
        {
            string sensorName = sensorType.Name;
            string sensorLayerName = R.S.Layer.ToString(sensorLayer);

            if (LayerMask.NameToLayer(sensorLayerName) == -1)
            {
                DrawErrorBox("In order to use a " + sensorName + ", you must have a " + sensorLayerName + " layer.");
            }

            DrawWarningBox("At start, a " + sensorName + " sets the layer of the GameObject to " + sensorLayerName + ". " +
                           "You should make sure that your " + sensorName + " is in it own GameObject to avoid any " +
                           "conflict.\n\nAlso, the only layer that should collide with the layer " + sensorLayerName +
                           " is itself. Change that in the Physics2D settings.");
        }
    }
}