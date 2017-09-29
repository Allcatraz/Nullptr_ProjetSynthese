using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using UnityEditor;
using UnityEngine;

namespace ProjetSynthese
{
    [CustomEditor(typeof(GameScript), true)]
    public class GameScriptInspector : Inspector
    {
        private GameScript gameScript;

        protected void Awake()
        {
            gameScript = target as GameScript;
        }

        protected void OnDestroy()
        {
            gameScript = null;
        }

        protected override void OnDraw()
        {
            DrawDefaultInspector();
            //In play mode, inspector is refreshed every frame. To prevent performance drop, only do this
            //while not in play mode.
            if (!EditorApplication.isPlaying)
            {
                ScopeSet scopes = new ScopeSet();
                List<string> unscopedDependencies = new List<string>();

                foreach (MethodInfo method in target.GetType().GetMethods(BindingFlags.Public |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.Instance))
                {
                    if (method.Name.ToLower().StartsWith("inject") && method.Name != "InjectDependencies")
                    {
                        foreach (ParameterInfo parameter in method.GetParameters())
                        {
                            Scope scope = GetScopeName(parameter);

                            if (scope != null)
                            {
                                bool isResolved;
                                try
                                {
                                    isResolved = scope.GetDependencies(gameScript, parameter.ParameterType).Count > 0;
                                }
                                catch (Exception)
                                {
                                    isResolved = false;
                                }
                                scopes[scope.GetType().Name].Add(parameter.ParameterType.Name, isResolved);
                            }
                            else
                            {
                                unscopedDependencies.Add(parameter.ParameterType.Name);
                            }
                        }
                    }
                }

                if (unscopedDependencies.Count > 0)
                {
                    string unscopedDependenciesText = "Unscoped Dependencies : \n";
                    foreach (string dependency in unscopedDependencies)
                    {
                        unscopedDependenciesText += "\n ● " + dependency;
                    }
                    DrawErrorBox(unscopedDependenciesText);
                }

                if (!scopes.IsEmpty())
                {
                    BeginTable("Dependencies");
                    foreach (DependencySet scope in scopes)
                    {
                        BeginTableRow();
                        DrawTableHeader(scope.Name);
                        foreach (Dependency dependency in scope)
                        {
                            if (dependency.IsResolved)
                            {
                                DrawTableCell("✓" + dependency.Name, new Color(0.05f, 0.5F, 0.05f));
                            }
                            else
                            {
                                DrawTableCell("✗" + dependency.Name, Color.red);
                            }
                        }
                        EndTableRow();
                    }
                    EndTable();
                }
            }
        }

        private static Scope GetScopeName(ParameterInfo parameter)
        {
            //Scope attribute is only allowed once, so it's safe to assume that if we find one, we get only one.
            foreach (object attribute in parameter.GetCustomAttributes(true))
            {
                if (attribute is Scope)
                {
                    return attribute as Scope;
                }
            }
            return null;
        }

        private class ScopeSet : IEnumerable<DependencySet>
        {
            private readonly Dictionary<string, DependencySet> scopes;

            public ScopeSet()
            {
                scopes = new Dictionary<string, DependencySet>();
            }

            public DependencySet this[string name]
            {
                get
                {
                    if (!scopes.ContainsKey(name))
                    {
                        scopes.Add(name, new DependencySet(name));
                    }
                    return scopes[name];
                }
            }

            public bool IsEmpty()
            {
                return scopes.Count == 0;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<DependencySet> GetEnumerator()
            {
                return scopes.Values.GetEnumerator();
            }
        }

        private class DependencySet : IEnumerable<Dependency>
        {
            private readonly HashSet<Dependency> dependencies;

            public string Name { get; private set; }

            public DependencySet(string name)
            {
                Name = name;
                dependencies = new HashSet<Dependency>();
            }

            public void Add(string name, bool isResolved)
            {
                dependencies.Add(new Dependency(name, isResolved));
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<Dependency> GetEnumerator()
            {
                return dependencies.GetEnumerator();
            }
        }

        private class Dependency
        {
            public string Name { get; private set; }
            public bool IsResolved { get; private set; }

            public Dependency(string name, bool isResolved)
            {
                Name = name;
                IsResolved = isResolved;
            }
        }
    }
}