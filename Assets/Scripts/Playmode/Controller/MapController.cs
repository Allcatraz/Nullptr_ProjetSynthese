using Castle.Core.Internal;
using Harmony;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class MapController : GameScript
    {
        [SerializeField] private RectTransform mapRectTransform;
        [SerializeField] private Camera mapCamera;

        public RectTransform MapRectTransform
        {
            get
            {
                return mapRectTransform;
            }
        }

        private TiledMap tileMap;

        private void Awake()
        {
            GameObject[] gameObjects = SceneManager.GetSceneByName(R.S.Scene.GameFragment).GetRootGameObjects();
            tileMap = gameObjects.Find(obj => obj.name == "Map").GetComponent<TiledMap>();
        }
            
        private void Start()
        {
            RenderTexture mapTexture = new RenderTexture(tileMap.MapWidthInPixels, tileMap.MapHeightInPixels, 24);
            GetComponent<RawImage>().texture = mapTexture;

            float widthScale = (float)Screen.width / tileMap.MapWidthInPixels;
            float heightScale = (float)Screen.height / tileMap.MapHeightInPixels;
            float scale = widthScale < heightScale ? widthScale : heightScale;

            mapRectTransform.localScale = new Vector3(scale, scale, 0);
            mapRectTransform.sizeDelta = new Vector2(tileMap.MapWidthInPixels, tileMap.MapHeightInPixels);
            //mapRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

            mapCamera.transform.position = new Vector3(tileMap.MapWidthInPixels / 2.0f, 100, -tileMap.MapWidthInPixels / 2.0f);
            mapCamera.orthographicSize = tileMap.MapWidthInPixels / 2.0f;
            mapCamera.targetTexture = mapTexture;
            mapCamera.gameObject.SetActive(true);
        }
    }
}
