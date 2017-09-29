using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Harmony
{
#if UNITY_EDITOR_WIN
    /// <summary>
    /// Script Editor Unity déclanchant la génération des BuildSettings et des classes de constantes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Android possède probablement le <i>Build Process</i> le mieux conçu à ce jour : très solide, facilement configurable
    /// et conçu pour éliminer le plus d'irritants possible lors du développement. Ce n'est pas le cas de Unity, malgré la quantité 
    /// importante d'outils fournis avec le moteur.
    /// </para>
    /// <para>
    /// L'une des forces du <i>Build Process</i> Android est la gestion très stricte des ressources et des identifiants de ressources. 
    /// Toute tentative d'accès à une ressource non existante est souvent détectée dès la compilation, pourvu que l'on passe par 
    /// les moyens traditionnels. Par exemple :
    /// </para>
    /// <code>
    /// public class MainActivity extends AppCompatActivity {
    ///     private EditText inputEditText;
    ///     private TextView outputTextView;
    ///     
    ///     @Override
    ///     protected void onCreate(Bundle savedInstanceState) {
    ///         super.onCreate(savedInstanceState);
    ///         setContentView(R.layout.activity_main);
    ///     
    ///         inputEditText = (EditText)findViewById(R.id.inputEditText);
    ///         outputTextView = (TextView)findViewById(R.id.outputTextView);
    ///     }
    ///     
    ///     public void onButtonClicked(View view) {
    ///         String format = getString(R.string.text_output);
    ///         String output = String.format(format, inputEditText.getText().toString());
    ///         outputTextView.setText(output);
    ///     }   
    /// }
    /// </code>
    /// <para>
    /// Notez la classe <c>R</c>. Sous Android, cette classe est générée automatiquement à partir des ressources du projet et contient
    /// les identifiants des ressources. Utiliser une ressource inexistante devient donc presque impossible, car cela en résulterait d'une erreur
    /// à la compilation.
    /// </para>
    /// <para>
    /// Cependant, sous Unity, il y a rien de tel : l'accès à une ressource non existante n'est détectée que lors de l'exécution. 
    /// Par exemple, obtenir un <i>GameObject</i> ainsi peut causer un <i>NullReferenceException</i> :
    /// </para>
    /// <code>
    /// public class GameScript : MonoBehaviour
    /// {
    /// 	private GameObject player;
    /// 
    ///     public void Awake()
    ///     {
    ///         player = GameObject.Find("Player");
    ///     }
    ///     
    ///     public void Update()
    ///     {
    ///         player.DoSomething();
    ///     }
    /// }
    /// </code>
    /// <para>
    /// Pour résoudre ce problème, Harmony génère des classes de constantes. Ces constantes sont extraites directement du projet Unity 
    /// et mises à jour à chaque fois qu'un paramètre change ou qu'une scène est modifiée.
    /// </para>
    /// <para>
    /// Plusieurs classes de constantes sont générées :
    /// <list type="bullet">
    /// <item>Tag : Les<i>Tags</i> du projet.</item>
    /// <item>Layer : Les <i>Layers</i> du projet.</item>
    /// <item>Scene : Les scènes du projet.</item>
    /// <item>Prefab : Les <i>Prefabs</i> du projet.</item>
    /// <item>GameObject : Les <i>GameObjects</i> de toutes les scènes du projet (incluant ceux dans les <i>prefabs</i>).</item>
    /// </list>
    /// </para>
    /// <para>
    /// Chaque classe de constantes possède deux variantes : sous forme d'énumération et sous forme de string. Il est possible
    /// de passer de l'un à l'autre en utilisant les méthodes statiques <i>ToString</i> disponibles dans la variante sous forme
    /// de string.
    /// </para>
    /// <para>
    /// Tout comme pour Android, toutes ces constantes sont situées dans la classe <i>R</i>. Pour accéder aux enumérations, il faut
    /// ensuite naviguer dans la classe <i>E</i>. Pour les strings, il faut plutôt utiliser la classe <i>S</i>.
    /// </para>
    /// <para>
    /// Par exemple : <c>R.E.Tag.MainCamera</c> ou <c>R.S.Tag.MainCamera</c>.
    /// </para>
    /// <para>
    /// Il est donc possible d'utiliser ces constantes ainsi :
    /// </para>
    /// <code>
    /// public class GameScript : MonoBehaviour
    /// {
    /// 	private GameObject player;
    /// 
    ///     public void Awake()
    ///     {
    ///         player = GameObject.Find(R.S.GameObject.PLAYER);
    ///     }
    ///     
    ///     public void Update()
    ///     {
    ///         player.DoSomething();
    ///     }
    /// }
    /// </code>
    /// <para>
    /// En somme, le code généré ressemble à ceci :
    /// </para>
    /// <code>
    /// ----- AUTO GENERATED CODE - ANY MODIFICATION WILL BE OVERRIDEN ----- //
    ///
    ///using System;
    ///
    ///namespace Harmony
    ///{
    ///    public static class R
    ///    {
    ///        public static class E
    ///        {
    ///            public enum Layer
    ///            {
    ///                None = -1503227594,
    ///                Default = 1948333211,
    ///                Player = -2033144378,
    ///            }
    ///
    ///            public enum Tag
    ///            {
    ///                None = -1503227594,
    ///                ApplicationDependencies = 1730835176,
    ///                MainCamera = 1926007183,
    ///            }
    ///
    ///            public enum Scene
    ///            {
    ///                None = -1503227594,
    ///                Main = 1222789680,
    ///                PauseMenu = -1383623743,
    ///            }
    ///
    ///            public enum Prefab
    ///            {
    ///                None = -1503227594,
    ///                Player = -2033144378,
    ///                Rock = 1167972425,
    ///            }
    ///
    ///            public enum GameObject
    ///            {
    ///                None = -1503227594,
    ///                MainCamera = 1926007183,
    ///                Database = 1827775453,
    ///                PlayerSpawnPoint = 1207841199,
    ///            }
    ///
    ///        }
    ///
    ///        public static class S
    ///        {
    ///            public static class Layer
    ///            {
    ///                public const string None = "None";
    ///                public const string Player = "Player";
    ///
    ///                public static string ToString(E.Layer value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case E.Layer.None:
    ///                            return None;
    ///                        case E.Layer.Default:
    ///                            return Default;
    ///                        case E.Layer.Player:
    ///                            return Player;
    ///                    }
    ///                    return null;
    ///                }
    ///
    ///                public static E.Layer ToEnum(string value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case None:
    ///                            return E.Layer.None;
    ///                        case Default:
    ///                            return E.Layer.Default;
    ///                        case Player:
    ///                            return E.Layer.Player;
    ///                    }
    ///                    throw new ArgumentException("Unable to convert " + value + " to UnityRessources.Enums.Layer.");
    ///                }
    ///            }
    ///
    ///            public static class Tag
    ///            {
    ///                public const string None = "None";
    ///                public const string ApplicationDependencies = "Application Dependencies";
    ///                public const string MainCamera = "MainCamera";
    ///
    ///                public static string ToString(E.Tag value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case E.Tag.None:
    ///                            return None;
    ///                        case E.Tag.ApplicationDependencies:
    ///                            return ApplicationDependencies;
    ///                        case E.Tag.MainCamera:
    ///                            return MainCamera;
    ///                    }
    ///                    return null;
    ///                }
    ///
    ///                public static E.Tag ToEnum(string value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case None:
    ///                            return E.Tag.None;
    ///                        case ApplicationDependencies:
    ///                            return E.Tag.ApplicationDependencies;
    ///                        case MainCamera:
    ///                            return E.Tag.MainCamera;
    ///                    }
    ///                    throw new ArgumentException("Unable to convert " + value + " to UnityRessources.Enums.Tag.");
    ///                }
    ///            }
    ///
    ///            public static class Scene
    ///            {
    ///                public const string None = "None";
    ///                public const string Main = "Main";
    ///                public const string PauseMenu = "PauseMenu";
    ///
    ///                public static string ToString(E.Scene value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case E.Scene.None:
    ///                            return None;
    ///                        case E.Scene.Main:
    ///                            return Main;
    ///                        case E.Scene.PauseMenu:
    ///                            return PauseMenu;
    ///                    }
    ///                    return null;
    ///                }
    ///
    ///                public static E.Scene ToEnum(string value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case None:
    ///                            return E.Scene.None;
    ///                        case Main:
    ///                            return E.Scene.Main;
    ///                        case PauseMenu:
    ///                            return E.Scene.PauseMenu;
    ///                    }
    ///                    throw new ArgumentException("Unable to convert " + value + " to UnityRessources.Enums.Scene.");
    ///                }
    ///            }
    ///
    ///            public static class Prefab
    ///            {
    ///                public const string None = "None";
    ///                public const string Player = "Player";
    ///                public const string Rock = "Rock";
    ///
    ///                public static string ToString(E.Prefab value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case E.Prefab.None:
    ///                            return None;
    ///                        case E.Prefab.Player:
    ///                            return Player;
    ///                        case E.Prefab.Rock:
    ///                            return Rock;
    ///                    }
    ///                    return null;
    ///                }
    ///
    ///                public static E.Prefab ToEnum(string value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case None:
    ///                            return E.Prefab.None;
    ///                        case Player:
    ///                            return E.Prefab.Player;
    ///                        case Rock:
    ///                            return E.Prefab.Rock;
    ///                    }
    ///                    throw new ArgumentException("Unable to convert " + value + " to UnityRessources.Enums.Prefab.");
    ///                }
    ///            }
    ///
    ///            public static class GameObject
    ///            {
    ///                public const string None = "None";
    ///                public const string MainCamera = "MainCamera";
    ///                public const string Database = "Database";
    ///                public const string PlayerSpawnPoint = "PlayerSpawnPoint";
    ///
    ///                public static string ToString(E.GameObject value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case E.GameObject.None:
    ///                            return None;
    ///                        case E.GameObject.MainCamera:
    ///                            return MainCamera;
    ///                        case E.GameObject.Database:
    ///                            return Database;
    ///                        case E.GameObject.PlayerSpawnPoint:
    ///                            return PlayerSpawnPoint;
    ///                    }
    ///                    return null;
    ///                }
    ///
    ///                public static E.GameObject ToEnum(string value)
    ///                {
    ///                    switch (value)
    ///                    {
    ///                        case None:
    ///                            return E.GameObject.None;
    ///                        case MainCamera:
    ///                            return E.GameObject.MainCamera;
    ///                        case Database:
    ///                            return E.GameObject.Database;
    ///                        case PlayerSpawnPoint:
    ///                            return E.GameObject.PlayerSpawnPoint;
    ///                    }
    ///                    throw new ArgumentException("Unable to convert " + value + " to UnityRessources.Enums.GameObject.");
    ///                }
    ///            }
    ///
    ///        }
    ///    }
    ///}
    /// </code>
    /// <para>
    /// En utilisant ces constantes, si le nom d'un <i>Tag</i>, d'un <i>Layer</i> ou d'un <i>GameObject</i> change, alors la valeur et le nom de sa 
    /// constante changera aussi, et ce, automatiquement. L'erreur sera alors détectée à la compilation et non pas à l'exécution.
    /// </para>
    /// <para>
    /// La génération de code se déclanche soit :
    /// </para>
    /// <list type="bullet">
    /// <item>Au lancement de Unity.</item>
    /// <item>Lors de la sauvegarde d'un fichier.</item>
    /// <item>Lors de la compilation.</item>
    /// </list>
    /// <para>
    /// Prenez note que la génération de code n'est pas effectuée par cette classe, mais par un programme externe. Ce programme doit être
    /// situé à la racine du projet, à <c>PROJECT_ROOT/BuildTools/Harmony/HarmonyCodeGenerator.exe</c>.
    /// </para>
    /// </remarks>
    [InitializeOnLoad]
    public static class AssetsGenerator
    {
        static AssetsGenerator()
        {
            //Generate only const classes on Asset reload. Generating Build Settings can cause more trouble than anything.
            GenerateConstClasses();
        }

        [MenuItem("Assets/Generate Build Settings From Activities")]
        public static void GenerateBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();

            //Add Main scene
            scenes.Add(new EditorBuildSettingsScene(AssetsExtensions.FindScenePath(R.S.Scene.Main), true));

            //Add Util scenes
            foreach (string scenePath in AssetsExtensions.FindScenesPathIn("Scenes/Util"))
            {
                scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }

            //Add Activity scenes
            foreach (Activity activity in AssetsExtensions.FindAssets<Activity>())
            {
                if (activity.Scene != R.E.Scene.None)
                {
                    scenes.Add(new EditorBuildSettingsScene(AssetsExtensions.FindScenePath(R.S.Scene.ToString(activity.Scene)), true));
                }

                foreach (Fragment fragment in activity.Fragments)
                {
                    if (fragment.Scene != R.E.Scene.None)
                    {
                        scenes.Add(new EditorBuildSettingsScene(AssetsExtensions.FindScenePath(R.S.Scene.ToString(fragment.Scene)), true));
                    }
                }

                foreach (Menu menu in activity.Menus)
                {
                    if (menu.Scene != R.E.Scene.None)
                    {
                        scenes.Add(new EditorBuildSettingsScene(AssetsExtensions.FindScenePath(R.S.Scene.ToString(menu.Scene)), true));
                    }
                }
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        [MenuItem("Assets/Generate Const Classes")]
        public static void GenerateConstClasses()
        {
            //Prevent Unity from compiling code while generating more code.
            EditorApplication.LockReloadAssemblies();

            string pathToCodeGenerator = Path.GetFullPath(Path.Combine(Application.dataPath, "../BuildTools/Harmony/HarmonyCodeGenerator.exe"));
            string pathToProjectDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string pathToGeneratedDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, "Generated"));

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = pathToCodeGenerator,
                Arguments = "\"" + pathToProjectDirectory + "\" \"" + pathToGeneratedDirectory + "\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process codeGenerationProcess = Process.Start(processStartInfo);
            if (codeGenerationProcess != null)
            {
                codeGenerationProcess.WaitForExit();
            }
            else
            {
                Debug.Log("Code Generation is probably complete, but Unity doesn't know it yet. Please save your work to " +
                          "let Unity reload and compile the generated code.");
            }

            EditorApplication.UnlockReloadAssemblies();

            try
            {
                AssetDatabase.Refresh();
            }
            catch (Exception)
            {
                //Sometimes, this line causes a NullReferenceException. This is a known issue in Unity. Just ignore it.
            }
        }

        public static void Generate()
        {
            GenerateBuildSettings();
            GenerateConstClasses();
        }
    }

    /// <summary>
    /// Trigger Unity. Déclanché avant un build. Démarre la génération de code.
    /// </summary>
    /// <see cref="AssetsGenerator"/>
    public class GenerateCodeBeforeBuild : IPreprocessBuild
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            AssetsGenerator.Generate();
        }
    }

    /// <summary>
    /// Trigger Unity. Déclanché avant la sauvegarde d'un fichier. Démarre la génération de code.
    /// </summary>
    /// <see cref="AssetsGenerator"/>
    public class GenerateCodeOnSave : UnityEditor.AssetModificationProcessor
    {
        //Resharper seems wrong about this. Warnings are disabled.

        // ReSharper disable once Unity.InvalidStaticModifier
        // ReSharper disable once Unity.InvalidReturnType
        public static string[] OnWillSaveAssets(string[] paths)
        {
            AssetsGenerator.Generate();
            return paths;
        }
    }
#endif
}