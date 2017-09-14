using Harmony.Injection;
using ProjetSynthese;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Harmony.Unity
{
    [CustomEditor(typeof(UnityActivity))]
    public class UnityActivityInspector : UnityInspector
    {
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
                DrawButton("Stop Activity", StopPlaymode);
                DrawDisabledButton("Open Activity");
            }
            EndHorizontal();

            DrawEnumPropertyGrid(GetEnumProperty("scene", typeof(R.E.Scene)), 2);
            DrawEnumPropertyGrid(GetEnumProperty("controller", typeof(R.E.GameObject)), 2);
            DrawListProperty(GetListProperty("fragments"));
            DrawListProperty(GetListProperty("menus"));
            DrawBasicPropertyTitleLabel(GetBasicProperty("activeFragmentOnLoad"));
        }

        private void PlayInEditor()
        {
            if (!EditorApplication.isPlaying)
            {
                //The Application scene must allways be loaded
                OpenSceneInEditor(R.E.Scene.Application, OpenSceneMode.Single);

                //Tell the EditorActivityConfiguration to load a specific activity
                GameObject.FindGameObjectWithTag(R.S.Tag.ApplicationDependencies).
                    GetComponentInChildren<EditorActivityConfiguration>().Activity = target as UnityActivity;

                EditorApplication.isPlaying = true;
            }
        }

        private void StopPlaymode()
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
                OpenFragmentInEditor(fragments.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue as IFragment,
                                     OpenSceneMode.Additive);
            }

            //Load Activity menus
            var menus = GetListProperty("menus");
            for (int i = 0; i < menus.serializedProperty.arraySize; i++)
            {
                OpenMenuInEditor(menus.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue as IMenu,
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

        private void OpenFragmentInEditor(IFragment fragment, OpenSceneMode mode)
        {
            OpenSceneInEditor(fragment.Scene, mode);
        }

        private void OpenMenuInEditor(IMenu menu, OpenSceneMode mode)
        {
            OpenSceneInEditor(menu.Scene, mode);
        }
    }
}