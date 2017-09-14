using Harmony;
using Harmony.Injection;
using Harmony.Testing;
using UnityEngine;

namespace ProjetSynthese
{
    [NotTested(Reason.Factory, Reason.ContainsUnmockable)]
    [AddComponentMenu("Game/World/UI/Actuator/HighScoreViewSpawner")]
    public class HighScoreViewSpawner : GameScript
    {
        [SerializeField]
        private GameObject highScoreViewPrefab;

        private IPrefabFactory prefabFactory;

        public void InjectHighScoreViewSpawner(GameObject highScoreViewPrefab,
                                               [ApplicationScope] IPrefabFactory prefabFactory)
        {
            this.highScoreViewPrefab = highScoreViewPrefab;
            this.prefabFactory = prefabFactory;
        }

        public void Awake()
        {
            InjectDependencies("InjectHighScoreViewSpawner", highScoreViewPrefab);
        }

        public virtual void Spawn(GameObject contentView, HighScore highScore)
        {
            GameObject view = prefabFactory.Instantiate(highScoreViewPrefab,
                                                        Vector3.zero,
                                                        Quaternion.Euler(Vector3.zero),
                                                        contentView);

            Configure(view, highScore);
        }

        private void Configure(GameObject view, HighScore highScore)
        {
            HighScoreView highScoreView = view.GetComponentInChildren<HighScoreView>();
            highScoreView.SetHighScore(highScore);
        }
    }
}