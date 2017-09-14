using System.Collections.Generic;
using Harmony.Testing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Hierachy Unity.
    /// </summary>
    /// <inheritdoc cref="IHierachy"/>
    [NotTested(Reason.Wrapper)]
    [AddComponentMenu("Game/Utils/UnityHierachy")]
    public class UnityHierachy : IHierachy
    {
        public bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            if (!IsSceneLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName, mode);
            }
        }

        public void UnloadScene(string sceneName)
        {
#pragma warning disable 618
            SceneManager.UnloadScene(sceneName);
#pragma warning restore 618
        }

        public void SetActiveScene(Scene scene)
        {
            SceneManager.SetActiveScene(scene);
        }

        public void DestroyGameObject(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }

        public IList<GameObject> FindGameObjectsWithTag(string tag)
        {
            return new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
        }
    }
}