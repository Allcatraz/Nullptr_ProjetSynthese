using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/HighScoreViewSpawner")]
    public class HighScoreViewSpawner : GameScript
    {
        [SerializeField]
        private GameObject highScoreViewPrefab;

        public void Spawn(GameObject contentView, HighScore highScore)
        {
            GameObject view = Instantiate(highScoreViewPrefab,
                                          Vector3.zero,
                                          Quaternion.Euler(Vector3.zero),
                                          contentView.transform);

            Configure(view, highScore);
        }

        private void Configure(GameObject view, HighScore highScore)
        {
            HighScoreView highScoreView = view.GetComponentInChildren<HighScoreView>();
            highScoreView.SetHighScore(highScore);
        }
    }
}