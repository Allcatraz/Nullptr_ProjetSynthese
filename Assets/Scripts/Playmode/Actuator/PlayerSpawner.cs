using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/PlayerSpawner")]
    public class PlayerSpawner : GameScript
    {
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private GameObject playerSpawnPoint;

        public void Spawn()
        {
            GameObject player = Instantiate(playerPrefab,
                                            playerSpawnPoint.transform.position,
                                            Quaternion.Euler(Vector3.zero));

            Configure(player);
        }

        private void Configure(GameObject player)
        {
            PlayerController playerController = player.GetComponentInChildren<PlayerController>();
            playerController.Configure();
        }
    }
}