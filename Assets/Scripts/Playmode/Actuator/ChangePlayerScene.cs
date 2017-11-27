using Castle.Core.Internal;
using Harmony;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    //BEN_CORRECTION : Deux (trois ?) responsabilités différentes dans cette classe.
    
    public class ChangePlayerScene : GameScript
    {
        [Tooltip("Le menu de la map du joueur pour pouvoir apparaître.")]
        [SerializeField] private Menu mapMenu;

        private ActivityStack activityStack;
        private TiledMap tileMap;
        private RectTransform mapRectTransform;

        private bool hasMapMenuLoaded = true;
        private float left = 0;
        private float bottom = 0;
        private float right = 0;
        private float top = 0;

        private void InjectChangePlayerScene([ApplicationScope] ActivityStack activityStack)
        {
            this.activityStack = activityStack;
        }

        private void Awake()
        {
            InjectDependencies("InjectChangePlayerScene");
        }

        private void Update()
        {
            if (!activityStack.HasActivityLoading())
            {
                //BEN_CORRECTION : Nom trompeur. Lis le nom de ta variable et regarde ce qu'elle fait. Complètement l'inverse.
                if (hasMapMenuLoaded)
                {
                    activityStack.StartMenu(mapMenu);
                    GameObject[] gameObjects = SceneManager.GetSceneByName(R.S.Scene.GameFragment).GetRootGameObjects();
                    tileMap = gameObjects.Find(obj => obj.name == "Map").GetComponent<TiledMap>();

                    gameObjects = SceneManager.GetSceneByName(R.S.Scene.MapMenu).GetRootGameObjects();
                    mapRectTransform = gameObjects.Find(obj => obj.name == "Canvas").GetComponentInChildren<MapController>().MapRectTransform;

                    left = mapRectTransform.position.x + mapRectTransform.offsetMin.x * mapRectTransform.localScale.x;
                    bottom = mapRectTransform.position.y + mapRectTransform.offsetMin.y * mapRectTransform.localScale.y;
                    right = mapRectTransform.position.x + mapRectTransform.offsetMax.x * mapRectTransform.localScale.x;
                    top = mapRectTransform.position.y + mapRectTransform.offsetMax.y * mapRectTransform.localScale.y;

                    hasMapMenuLoaded = false;
                }

                Vector3 mousePos = Input.mousePosition;
                if (mousePos.x >= left && mousePos.x <= right && mousePos.y >= bottom && mousePos.y <= top && Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                {
                    Vector2 tiledMapScaled = new Vector2((float)tileMap.MapWidthInPixels / tileMap.TileWidth, (float)tileMap.MapHeightInPixels / tileMap.TileHeight);
                    Vector3 viewportPosition = new Vector3((mousePos.x - left) / (right - left), (mousePos.y - bottom) / (top - bottom));
                    viewportPosition.y = 1 - viewportPosition.y;
                    Vector3 worldPos = new Vector3(viewportPosition.x * tiledMapScaled.x, 0.5f, -viewportPosition.y * tiledMapScaled.y);

                    activityStack.StopCurrentMenu();
                    transform.position = worldPos;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(R.S.Scene.GameFragment));

                    Destroy(this);
                }
            }
        }
    }
}