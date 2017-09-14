using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Transform Unity.
    /// </summary>
    /// <inheritdoc cref="ITransform"/>
    [NotTested(Reason.Wrapper)]
    public class UnityTransform : UnityComponent, ITransform
    {
        private readonly Transform transform;

        public UnityTransform(Transform transform) : base(transform)
        {
            this.transform = transform;
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Vector3 LocalPosition
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }

        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        public Quaternion LocalRotation
        {
            get { return transform.localRotation; }
            set { transform.localRotation = value; }
        }

        public Vector3 Scale
        {
            get { return transform.lossyScale; }
        }

        public Vector3 LocalScale
        {
            get { return transform.localScale; }
            set { transform.localScale = value; }
        }

        public Vector3 Up
        {
            get { return transform.up; }
        }

        public void Translate(Vector3 translation)
        {
            transform.Translate(translation);
        }
    }
}