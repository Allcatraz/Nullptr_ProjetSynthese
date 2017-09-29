using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;

namespace Harmony
{
    /// <summary>
    /// Inspecteur pour les Activités dans l'éditeur Unity.
    /// </summary>
    [CustomEditor(typeof(Activity), true)]
    public class ActivityInspector : Inspector
    {
        private EnumProperty scene;
        private EnumProperty controller;
        private ReorderableList fragments;
        private ReorderableList menus;
        private SerializedProperty activeFragmentOnLoad;

        private void Awake()
        {
            scene = GetEnumProperty("scene", typeof(R.E.Scene));
            controller = GetEnumProperty("controller", typeof(R.E.GameObject));
            fragments = GetListProperty("fragments");
            menus = GetListProperty("menus");
            activeFragmentOnLoad = GetBasicProperty("activeFragmentOnLoad");
        }

        private void OnDestroy()
        {
            scene = null;
            controller = null;
            fragments = null;
            menus = null;
            activeFragmentOnLoad = null;
        }

        protected override void OnDraw()
        {
            DrawTitleLabel("Activity tools");
            BeginHorizontal();
            if (!EditorApplication.isPlaying)
            {
                DrawButton("Open Activity", OpenActivityInEditor);
            }
            else
            {
                DrawDisabledButton("Open Activity");
            }
            EndHorizontal();

            DrawEnumPropertyGrid(scene, 2);
            DrawEnumPropertyGrid(controller, 2);
            DrawListProperty(fragments);
            DrawListProperty(menus);
            DrawBasicPropertyTitleLabel(activeFragmentOnLoad);
        }

        private void OpenActivityInEditor()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            //The Game scene must allways be loaded
            OpenSceneInEditor(R.E.Scene.Game, OpenSceneMode.Single);

            //Load Activity controller scene
            R.E.Scene scene = (R.E.Scene) GetEnumProperty("scene", typeof(R.E.Scene)).CurrentValue;
            if (scene != R.E.Scene.None)
            {
                OpenSceneInEditor(scene, OpenSceneMode.Additive);
            }

            //Load Activity fragments
            var fragments = GetListProperty("fragments");
            for (int i = 0; i < fragments.serializedProperty.arraySize; i++)
            {
                OpenFragmentInEditor(fragments.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue as Fragment,
                                     OpenSceneMode.Additive);
            }

            //Load Activity menus
            var menus = GetListProperty("menus");
            for (int i = 0; i < menus.serializedProperty.arraySize; i++)
            {
                OpenMenuInEditor(menus.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue as Menu,
                                 OpenSceneMode.Additive);
            }
        }

        private void OpenSceneInEditor(R.E.Scene scene, OpenSceneMode mode)
        {
            foreach (EditorBuildSettingsScene builtScene in EditorBuildSettings.scenes)
            {
                //The "/" in the front is to prevent some issues with similar scene names.
                //EX :
                //      NewHighScoreScene.unity and HighScoreScene.unity 
                if (builtScene.path.EndsWith("/" + R.S.Scene.ToString(scene) + ".unity"))
                {
                    EditorSceneManager.OpenScene(builtScene.path, mode);
                    break;
                }
            }
        }

        private void OpenFragmentInEditor(Fragment fragment, OpenSceneMode mode)
        {
            OpenSceneInEditor(fragment.Scene, mode);
        }

        private void OpenMenuInEditor(Menu menu, OpenSceneMode mode)
        {
            OpenSceneInEditor(menu.Scene, mode);
        }
    }
}