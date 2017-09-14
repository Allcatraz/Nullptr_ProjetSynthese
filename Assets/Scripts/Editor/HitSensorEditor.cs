﻿using Harmony;
using UnityEditor;
using UnityEngine;

namespace ProjetSynthese
{
    [CustomEditor(typeof(HitSensor), true)]
    public class HitSensorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (LayerMask.NameToLayer(R.S.Layer.HitSensor) == -1)
            {
                EditorGUILayout.HelpBox("In order to use a HitSensor, you must have a " + R.S.Layer.HitSensor +
                                        " layer.", MessageType.Error);
            }

            EditorGUILayout.HelpBox("At start, a HitSensor sets the layer of the GameObject to " + R.S.Layer.HitSensor + ". " +
                                    "You should make sure that your HitStimulus is in it own GameObject to avoid any " +
                                    "conflict.\n\nAlso, the only layer that should collide with the layer " + R.S.Layer.HitSensor +
                                    " is itself. Change that in the Physics2D settings.", MessageType.Info);
            DrawDefaultInspector();
        }
    }
}