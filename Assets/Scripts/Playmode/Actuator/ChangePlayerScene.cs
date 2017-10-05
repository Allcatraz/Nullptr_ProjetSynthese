using Castle.Core.Internal;
using Harmony;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    public class ChangePlayerScene : GameScript
    {
        [SerializeField] private Menu mapMenu;

        private ActivityStack activityStack;
        private TiledMap tileMap;
        private bool hasMapMenuLoaded = true;

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
                if (hasMapMenuLoaded)
                {
                    activityStack.StartMenu(mapMenu);
                    GameObject[] gameObjects = SceneManager.GetSceneByName(R.S.Scene.GameFragment).GetRootGameObjects();
                    tileMap = gameObjects.Find(obj => obj.name == "Map").GetComponent<TiledMap>();
                    hasMapMenuLoaded = false;
                }

                Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                viewportPosition.y = 1 - viewportPosition.y;
                Vector3 worldPos = new Vector3(viewportPosition.x * tileMap.MapWidthInPixels, 10, -viewportPosition.y * tileMap.MapHeightInPixels);
                if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
                {
                    activityStack.StopCurrentMenu();
                    transform.position = worldPos;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    SceneManager.MoveGameObjectToScene(gameObject.GetRoot(), SceneManager.GetSceneByName(R.S.Scene.GameFragment));
                    Destroy(this);
                }
            }
        }
    }
}

