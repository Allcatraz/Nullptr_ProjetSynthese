using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/LoadScenes")]
    public class LoadScenes : GameScript
    {
        [SerializeField]
        private R.E.Scene[] scenes;

        [SerializeField]
        private LoadSceneMode mode = LoadSceneMode.Additive;

        private void Start()
        {
            foreach (R.E.Scene scene in scenes)
            {
                SceneManager.LoadScene(R.S.Scene.ToString(scene), mode);
            }
        }
    }
}