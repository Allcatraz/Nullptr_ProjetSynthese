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
        private PlayerMoveEventChannel playerMoveEventChannel;

        private void InjectMinimapController([EntityScope] Camera minimapCam,
                                             [GameObjectScope] RectTransform rectTransform,
                                             [EventChannelScope] PlayerMoveEventChannel playerMoveEventChannel)
        {
            this.rectTransform = rectTransform;
            this.minimapCam = minimapCam;
            this.playerMoveEventChannel = playerMoveEventChannel;
        }
 
        private void Awake()
        {
            InjectDependencies("InjectMinimapController");
            playerMoveEventChannel.OnEventPublished += OnPlayerMove;
        }

        private void OnDestroy()
        {
            playerMoveEventChannel.OnEventPublished -= OnPlayerMove;
        }

        private void Start()
        {
            RenderTexture texture = new RenderTexture(minimapWidth, minimapHeight, 24);
            GetComponent<RawImage>().texture = texture;

            rectTransform.sizeDelta = new Vector2(minimapWidth, minimapHeight);

            minimapCam.targetTexture = texture;
        }

        private void OnPlayerMove(PlayerMoveEvent playerMoveEvent)
        {
            minimapCam.transform.position = new Vector3(playerMoveEvent.PlayerMover.transform.position.x, 100, playerMoveEvent.PlayerMover.transform.position.z);
        }
    }

}

