using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/WraparoundOnScreenExit")]
    public class WraparoundOnScreenExit : GameScript
    {
        private Renderer topParentRrenderer;
        private Transform topParentTranform;
        private new Camera camera;

        private void InjectWraparoundOnScreen([TopParentScope] Renderer topParentRrenderer,
                                             [TopParentScope] Transform topParentTranform,
                                             [TagScope(R.S.Tag.MainCamera)] Camera camera)
        {
            this.topParentRrenderer = topParentRrenderer;
            this.topParentTranform = topParentTranform;
            this.camera = camera;
        }

        private void Awake()
        {
            InjectDependencies("InjectWraparoundOnScreen");
        }

        private void OnEnable()
        {
            topParentRrenderer.Events().OnIsInvisible += OnOutOfScreen;
        }

        private void OnDisable()
        {
            topParentRrenderer.Events().OnIsInvisible -= OnOutOfScreen;
        }

        private void OnOutOfScreen()
        {
            topParentTranform.position = FlipPositionAccordingToCamera(topParentTranform.position, camera);
        }

        private Vector3 FlipPositionAccordingToCamera(Vector3 position, Camera camera)
        {
            Vector3 viewportPosition = camera.WorldToViewportPoint(position);
            float x = IsOutOfViewport(viewportPosition.x) ? -position.x : position.x;
            float y = IsOutOfViewport(viewportPosition.y) ? -position.y : position.y;
            return new Vector3(x, y, position.z);
        }

        private bool IsOutOfViewport(float viewportPosition)
        {
            return viewportPosition > 1 || viewportPosition < 0;
        }  
    }
}