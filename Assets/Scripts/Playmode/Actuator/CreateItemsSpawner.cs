using System.Collections.Generic;
using Castle.Core.Internal;
using Harmony;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    //BEN_REVIEW : Cela crée les "Spawners" ? Donc "CreateItemSpawners" non ?
    public class CreateItemsSpawner : GameScript
    {
        [Tooltip("Liste contenant toutes les positions des ItemsSpawner.")]
        [SerializeField] private List<Vector3> itemSpawnerPositions;
        [Tooltip("Le Prefab de l'ItemSpawner.")]
        [SerializeField] private GameObject itemSpawnerPrefab;

        private ActivityStack activityStack;
        private PlayerController playerController;

        private static int nbSpawn = 0; //BEN_CORRECTION : Ça sert à rien ça...

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
                    //BEN_REVIEW : Ne devrait-il pas se détruite tout de suite s'il n'est pas serveur ?
                    if (playerController.isServer)
                    {
                        GameObject[] gameObjects = SceneManager.GetSceneByName(R.S.Scene.GameFragment).GetRootGameObjects();
                        //BEN_CORRECTION : Remplacer "Map" par R.S.GameObject.Map. Voir plus bas.
                        TiledMap tileMap = gameObjects.Find(obj => obj.name == "Map").GetComponent<TiledMap>();
                        //TiledMap tileMap = gameObjects.Find(obj => obj.name == R.S.GameObject.Map).GetComponent<TiledMap>();

                        //BEN_CORRECTION : Nommage.
                        foreach (Vector3 vec3 in itemSpawnerPositions)
                        {
                            //BEN_REVIEW : Vous faites ça souvent (Instanciate suivi de NetworkServer.Spawn) ?
                            //             Pourquoi pas avoir un script dont le seul but est de faire ça justement ?
                            //             Vous n'auriez plus à penser à faire NetworkServer.Spawn après chaque Instanciate non ?
                            //
                            //             EDIT : Vous semblez déjà faire quelque chose du genre dans vos fabriques pourtant...
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
