using ProjetSynthese;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Inspecteur pour les Activités dans l'éditeur Unity.
    /// </summary>
    [CustomEditor(typeof(Activity))]
    public class UnityActivityInspector : UnityInspector
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
                DrawButton("Start Activity", PlayInEditor);
                DrawButton("Open Activity", OpenActivityInEditor);
            }
            else
            {
                DrawButton("Stop Activity", StopPlayInEditor);
                DrawDisabledButton("Open Activity");
            }
            EndHorizontal();

            DrawEnumPropertyGrid(scene, 2);
            DrawEnumPropertyGrid(controller, 2);
            DrawListProperty(fragments);
            DrawListProperty(menus);
            DrawBasicPropertyTitleLabel(activeFragmentOnLoad);
        }

        private void PlayInEditor()
        {
            if (!EditorApplication.isPlaying)
            {
                //The Application scene must allways be loaded
                OpenSceneInEditor(R.E.Scene.Application, OpenSceneMode.Single);

                //Tell the EditorActivityConfiguration to load a specific activity
                GameObject.FindGameObjectWithTag(R.S.Tag.ApplicationDependencies).GetComponentInChildren<EditorActivityConfiguration>().Activity =
                    target as Activity;

                EditorApplication.isPlaying = true;
            }
        }

        private void StopPlayInEditor()
        {
            EditorApplication.isPlaying = false;
        }

        private void OpenActivityInEditor()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            //The Application scene must allways be loaded
            OpenSceneInEditor(R.E.Scene.Application, OpenSceneMode.Single);

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
                                 OpenSceneMode.AdditiveWithoutLoading);
            }
        }

        private void OpenSceneInEditor(R.E.Scene scene, OpenSceneMode mode)
        {
            foreach (EditorBuildSettingsScene builtScene in EditorBuildSettings.scenes)
            {
                if (builtScene.path.EndsWith(R.S.Scene.ToString(scene) + ".unity"))
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