using Harmony;
using Harmony.Injection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Control/LoadScenes")]
    public class LoadScenes : GameScript
    {
        [SerializeField]
        private R.E.Scene[] scenes;

        [SerializeField]
        private LoadSceneMode mode = LoadSceneMode.Additive;

        private IHierachy hierachy;

        public void InjectLoadSceneAtStart(R.E.Scene[] scenes,
                                           LoadSceneMode mode,
                                           [ApplicationScope] IHierachy hierachy)
        {
            this.scenes = scenes;
            this.mode = mode;
            this.hierachy = hierachy;
        }

        public void Awake()
        {
            InjectDependencies("InjectLoadSceneAtStart", scenes, mode);
        }

        public void Start()
        {
            foreach (R.E.Scene scene in scenes)
            {
                hierachy.LoadScene(R.S.Scene.ToString(scene), mode);
            }
        }
    }
}