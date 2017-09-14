using UnityEditor;

namespace ProjetSynthese
{
    [CustomEditor(typeof(PrefabInstanceCollection), true)]
    public class PrefabInstanceCollectionEditor : Editor
    {
        private PrefabInstanceCollection prefabInstanceCollection;

        public void OnEnable()
        {
            prefabInstanceCollection = target as PrefabInstanceCollection;
        }

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Number of elements : " + prefabInstanceCollection.Count, MessageType.Info);
            }

            DrawDefaultInspector();
        }
    }
}