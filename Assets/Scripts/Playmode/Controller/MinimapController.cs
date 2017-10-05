using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class MinimapController : GameScript
    {
        [SerializeField] private int minimapWidth;
        [SerializeField] private int minimapHeight;

        private RectTransform rectTransform;
        private PlayerController playerController;
        private Camera minimapCam;

        private void InjectMinimapController([EntityScope] Camera minimapCam,
                                             [GameObjectScope] RectTransform rectTransform)
        {
            this.rectTransform = rectTransform;
            this.minimapCam = minimapCam;
        }
 
        private void Awake()
        {
            InjectDependencies("InjectMinimapController");
        }

        private void Start()
        {
            RenderTexture texture = new RenderTexture(minimapWidth, minimapHeight, 24);
            GetComponent<RawImage>().texture = texture;

            rectTransform.sizeDelta = new Vector2(minimapWidth, minimapHeight);

            minimapCam.targetTexture = texture;
        }

        private void FixedUpdate()
        {
            minimapCam.transform.position = new Vector3(StaticMinimapPass.PlayerTransform.position.x, 10, StaticMinimapPass.PlayerTransform.position.z);
        }
    }

}

