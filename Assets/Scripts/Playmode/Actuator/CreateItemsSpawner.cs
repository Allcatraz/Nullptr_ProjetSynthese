using System.Collections.Generic;
using Castle.Core.Internal;
using Harmony;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    public class CreateItemsSpawner : GameScript
    {
        [SerializeField] private List<Vector3> itemSpawnerPositions;
        [SerializeField] private GameObject itemSpawnerPrefab;

        private ActivityStack activityStack;
        private PlayerController playerController;

        private static int nbSpawn = 0;

        private void InjectCreateItemsSpawner([ApplicationScope] ActivityStack activityStack,
                                              [GameObjectScope] PlayerController playerController)
        {
            this.activityStack = activityStack;
            this.playerController = playerController;
        }

        private void Awake()
        {
            InjectDependencies("InjectCreateItemsSpawner");
        }

        void Update()
        {
            if (nbSpawn == 0)
            {
                if (!activityStack.HasActivityLoading())
                {
                    if (playerController.isServer)
                    {
                        GameObject[] gameObjects = SceneManager.GetSceneByName(R.S.Scene.GameFragment).GetRootGameObjects();
                        TiledMap tileMap = gameObjects.Find(obj => obj.name == "Map").GetComponent<TiledMap>();

                        foreach (Vector3 vec3 in itemSpawnerPositions)
                        {
                            GameObject itemSpawner = Instantiate(itemSpawnerPrefab, tileMap.transform);
                            NetworkServer.Spawn(itemSpawner);
                            ItemSpawnerController itemSpawnercontroller = itemSpawner.GetComponent<ItemSpawnerController>();
                            itemSpawnercontroller.CreateItems(vec3);
                        }
                        nbSpawn++;
                        Destroy(this);
                    }
                }
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
