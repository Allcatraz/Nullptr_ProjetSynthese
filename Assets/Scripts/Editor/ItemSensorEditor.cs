using Harmony;
using UnityEditor;
using UnityEngine;

namespace ProjetSynthese
{
    [CustomEditor(typeof(ItemSensor), true)]
    public class ItemSensorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (LayerMask.NameToLayer(R.S.Layer.HitSensor) == -1)
            {
                EditorGUILayout.HelpBox("In order to use a ItemSensor, you must have a " + R.S.Layer.ItemSensor +
                                        " layer.", MessageType.Error);
            }

            EditorGUILayout.HelpBox("At start, a ItemSensor sets the layer of the GameObject to " + R.S.Layer.ItemSensor + ". " +
                                    "You should make sure that your ItemSensor is in it own GameObject to avoid any " +
                                    "conflict.\n\nAlso, the only layer that should collide with the layer " + R.S.Layer.ItemSensor +
                                    " is itself. Change that in the Physics2D settings.", MessageType.Info);
            DrawDefaultInspector();
        }
    }
}