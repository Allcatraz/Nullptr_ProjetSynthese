using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/WraparoundOnScreenExit")]
    public class WraparoundOnScreenExit : GameScript
    {
        private new IRenderer renderer;
        private new ITransform transform;
        private new ICamera camera;

        public void InjectWraparoundOnScreen([TopParentScope] IRenderer renderer,
                                             [TopParentScope] ITransform transform,
                                             [TagScope(R.S.Tag.MainCamera)] ICamera camera)
        {
            this.renderer = renderer;
            this.transform = transform;
            this.camera = camera;
        }

        public void Awake()
        {
            InjectDependencies("InjectWraparoundOnScreen");
        }

        public void OnEnable()
        {
            renderer.OnBecameInvisible += OnOutOfScreen;
        }

        public void OnDisable()
        {
            renderer.OnBecameInvisible -= OnOutOfScreen;
        }

        private void OnOutOfScreen()
        {
            transform.Position = FlipPositionAccordingToCamera(transform.Position, camera);
        }

        private Vector3 FlipPositionAccordingToCamera(Vector3 position, ICamera camera)
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