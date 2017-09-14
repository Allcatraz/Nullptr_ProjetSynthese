using Harmony;
using Harmony.Injection;
using UnityEngine;
using Harmony.Testing;

namespace ProjetSynthese
{
    [NotTested(Reason.Factory, Reason.ContainsUnmockable)]
    [AddComponentMenu("Game/World/Object/Actuator/PlayerSpawner")]
    public class PlayerSpawner : GameScript
    {
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private GameObject playerSpawnPoint;

        private IPrefabFactory prefabFactory;

        public void InjectPlayerSpawner(GameObject playerPrefab,
                                        GameObject playerSpawnPoint,
                                        [ApplicationScope] IPrefabFactory prefabFactory)
        {
            this.playerPrefab = playerPrefab;
            this.playerSpawnPoint = playerSpawnPoint;
            this.prefabFactory = prefabFactory;
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayerSpawner",
                               playerPrefab,
                               playerSpawnPoint);
        }

        public virtual void Spawn()
        {
            GameObject player = prefabFactory.Instantiate(playerPrefab,
                                                          playerSpawnPoint.transform.position,
                                                          Quaternion.Euler(Vector3.zero));

            Configure(player, playerSpawnPoint.transform.position);
        }

        private void Configure(GameObject player, Vector3 position)
        {
            player.transform.position = position;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);

            PlayerController playerController = player.GetComponentInChildren<PlayerController>();
            playerController.Configure();
        }
    }
}