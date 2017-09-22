using Harmony;
using UnityEditor;
using UnityEngine;

namespace ProjetSynthese
{
    [CustomEditor(typeof(ItemStimulus), true)]
    public class ItemStimulusEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (LayerMask.NameToLayer(R.S.Layer.HitSensor) == -1)
            {
                EditorGUILayout.HelpBox("In order to use a ItemStimulus, you must have a " + R.S.Layer.ItemSensor +
                                        " layer.", MessageType.Error);
            }

            EditorGUILayout.HelpBox("At start, a ItemStimulus sets the layer of the GameObject to " + R.S.Layer.ItemSensor + ". " +
                                    "You should make sure that your ItemStimulus is in it own GameObject to avoid any " +
                                    "conflict.\n\nAlso, the only layer that should collide with the layer " + R.S.Layer.ItemSensor +
                                    " is itself. Change that in the Physics2D settings.", MessageType.Info);
            DrawDefaultInspector();
        }
    }
}