using Castle.Core.Internal;
using Harmony;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class MapSizeController : GameScript
    {
        [SerializeField] private RectTransform mapRectTransform;
        [SerializeField] private Camera mapCamera;

        private TiledMap tileMap;

        void Awake()
        {
            GameObject[] gameObjects = SceneManager.GetSceneByName(R.S.Scene.GameFragment).GetRootGameObjects();
            tileMap = gameObjects.Find(obj => obj.name == "Map").GetComponent<TiledMap>();
        }

        void Start()
        {
            RenderTexture mapTexture = new RenderTexture(tileMap.MapWidthInPixels, tileMap.MapHeightInPixels, 24);
            GetComponent<RawImage>().texture = mapTexture;

            mapRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

            mapCamera.transform.position = new Vector3(tileMap.MapWidthInPixels / 2.0f, 100, -tileMap.MapWidthInPixels / 2.0f);
            mapCamera.orthographicSize = tileMap.MapWidthInPixels / 2.0f;
            mapCamera.targetTexture = mapTexture;
            mapCamera.gameObject.SetActive(true);
        }
    }
}
