using Harmony;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    public class ChangePlayerScene : GameScript
    {
        [SerializeField] private Menu mapMenu;

        private ActivityStack activityStack;
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
                    hasMapMenuLoaded = false;
                }

                Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                viewportPosition.y = 1 - viewportPosition.y;
                Vector3 worldPos = new Vector3(viewportPosition.x * 800, 10, -viewportPosition.y * 800);

                //Vector2 proportionalPosition = new Vector2(ViewportPosition.x * Screen.width, ViewportPosition.y * Screen.height);
                //Vector3 proportionalPositionVec3 = new Vector3(proportionalPosition.x, 10, -proportionalPosition.y);
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

