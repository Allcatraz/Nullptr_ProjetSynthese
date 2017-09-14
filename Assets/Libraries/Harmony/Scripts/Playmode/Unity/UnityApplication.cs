using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente une application Unity.
    /// </summary>
    /// <inheritdoc cref="IApplication"/>
    [NotTested(Reason.Wrapper)]
    [AddComponentMenu("Game/Utils/UnityApplication")]
    public class UnityApplication : IApplication
    {
        public string ApplicationDataPath
        {
            get { return Application.streamingAssetsPath; }
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}