using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente une Camera Unity.
    /// </summary>
    /// <inheritdoc cref="ICamera"/>
    [NotTested(Reason.Wrapper)]
    public class UnityCamera : UnityDisableable, ICamera
    {
        private readonly Camera camera;

        public UnityCamera(Camera camera) : base(camera)
        {
            this.camera = camera;
        }

        public Vector3 Position
        {
            get { return camera.transform.position; }
            set { camera.transform.position = value; }
        }

        public float CameraHeightInWorldUnit
        {
            get { return (camera.ViewportToWorldPoint(Vector3.up) - camera.transform.position).y * 2; }
        }

        public float CameraWidthInWorldUnit
        {
            get { return (camera.ViewportToWorldPoint(Vector3.right) - camera.transform.position).x * 2; }
        }

        public Vector3 WorldToViewportPoint(Vector3 position)
        {
            return camera.WorldToViewportPoint(position);
        }

        public Vector3 ViewportToWorldPoint(Vector3 position)
        {
            return camera.ViewportToWorldPoint(position);
        }

        public Vector3 ScreenToWorldPoint(Vector3 position)
        {
            return camera.ScreenToWorldPoint(position);
        }

        
    }
}