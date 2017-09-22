using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace Harmony.Unity
{
    /// <summary>
    /// Base pour créer facilement un inspecteur personalisé sous Unity, avec quelques fonctionalitées supplémentaires.
    /// </summary>
    public abstract class UnityInspector : Editor
    {
        protected SerializedProperty GetBasicProperty(string name)
        {
            return serializedObject.FindProperty(name);
        }

        protected EnumProperty GetEnumProperty(string name, Type enumType)
        {
            return new EnumProperty(GetBasicProperty(name), enumType);
        }

        protected ReorderableList GetListProperty(string name)
        {
            SerializedProperty property = serializedObject.FindProperty(name);

            ReorderableList reorderableList = new ReorderableList(serializedObject,
                                                                  property,
                                                                  true,
                                                                  true,
                                                                  true,
                                                                  true);

            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty propertyListElement = property.GetArrayElementAtIndex(index);
                rect.y += 2; //Litle ajustment for esthetic purposes...
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                                        propertyListElement,
                                        GUIContent.none);
            };
            reorderableList.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, property.displayName); };

            return reorderableList;
        }

        protected void DrawBasicProperty(SerializedProperty property)
        {
            if (property != null && property.IsValid())
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        protected void DrawBasicPropertyTitleLabel(SerializedProperty property)
        {
            if (property != null && property.IsValid())
            {
                DrawTitleLabel(property.displayName);
                EditorGUILayout.PropertyField(property, GUIContent.none);
            }
        }

        protected void DrawListProperty(ReorderableList property)
        {
            if (property != null && property.IsValid())
            {
                DrawTitleLabel(property.serializedProperty.displayName);
                property.DoLayoutList();
            }
        }

        protected void DrawEnumPropertyDropDown(EnumProperty property)
        {
            if (property != null && property.IsValid())
            {
                BeginHorizontal();
                EditorGUILayout.PrefixLabel(property.Name);
                property.CurrentValueIndex = EditorGUILayout.Popup(property.CurrentValueIndex,
                                                                   property.ValuesNames);
                EndHorizontal();
            }
        }

        protected void DrawEnumPropertyGrid(EnumProperty property, int nbRows)
        {
            if (property != null && property.IsValid())
            {
                DrawTitleLabel(property.Name);
                property.CurrentValueIndex = GUILayout.SelectionGrid(property.CurrentValueIndex,
                                                                     property.ValuesNames,
                                                                     nbRows,
                                                                     EditorStyles.radioButton);
                EditorGUILayout.Space();
            }
        }

        protected void DrawButton(string text, UnityAction actionOnClick)
        {
            if (GUILayout.Button(text))
            {
                actionOnClick();
            }
        }

        protected void DrawDisabledButton(string text)
        {
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Button(text);
            EditorGUI.EndDisabledGroup();
        }

        protected void DrawTitleLabel(string text)
        {
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }

        protected void BeginHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
        }

        protected void EndHorizontal()
        {
            EditorGUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            OnDraw();
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void OnDraw();

        protected sealed class EnumPropertyValue
        {
            public int Value { get; private set; }
            public string Name { get; private set; }

            public EnumPropertyValue(int value, string name)
            {
                Value = value;
                Name = name;
            }
        }

        protected sealed class EnumProperty
        {
            private readonly SerializedProperty property;

            public string Name
            {
                get { return property.displayName; }
            }

            public List<EnumPropertyValue> Values { get; private set; }
            public string[] ValuesNames { get; private set; }

            public int CurrentValue
            {
                get { return property.intValue; }
                set { property.intValue = value; }
            }

            public int CurrentValueIndex
            {
                get
                {
                    for (int i = 0; i < Values.Count; i++)
                    {
                        if (CurrentValue == Values[i].Value)
                        {
                            return i;
                        }
                    }
                    return -1;
                }
                set { CurrentValue = Values[value].Value; }
            }

            public EnumProperty(SerializedProperty property, Type enumType)
            {
                this.property = property;

                //Enum values
                Array enumValues = Enum.GetValues(enumType);
                //Enum value names
                string[] enumNames = Enum.GetNames(enumType);

                //Create a list of enum values to display and sort it by name
                //If there is a "None" value, make it first.
                Values = new List<EnumPropertyValue>();
                int noneValueIndex = -1;
                for (int i = 0; i < enumValues.Length; i++)
                {
                    if (enumNames[i] == "None")
                    {
                        noneValueIndex = i;
                    }
                    else
                    {
                        Values.Add(new EnumPropertyValue((int) enumValues.GetValue(i),
                                                         enumNames[i]));
                    }
                }
                Values.Sort((displayable1, displayable2) => displayable1.Name.CompareTo(displayable2.Name));
                if (noneValueIndex != -1)
                {
                    Values.Insert(0, new EnumPropertyValue((int) enumValues.GetValue(noneValueIndex),
                                                           enumNames[noneValueIndex]));
                }

                //Create array of enum value names
                ValuesNames = new string[Values.Count];
                for (int i = 0; i < enumNames.Length; i++)
                {
                    ValuesNames[i] = Values[i].Name;
                }
            }

            public bool IsValid()
            {
                return property.IsValid();
            }

            public bool NeedRefresh()
            {
                return property.NeedRefresh();
            }
        }
    }
}