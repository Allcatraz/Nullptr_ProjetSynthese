using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un RigidBody2D Unity.
    /// </summary>
    /// <inheritdoc cref="IRigidbody2D"/>
    [NotTested(Reason.Wrapper)]
    public class UnityRigidbody2D : UnityComponent, IRigidbody2D
    {
        private readonly Rigidbody2D rigidbody2D;

        public UnityRigidbody2D(Rigidbody2D rigidbody2D) : base(rigidbody2D)
        {
            this.rigidbody2D = rigidbody2D;
        }

        public float Drag
        {
            get { return rigidbody2D.drag; }
            set { rigidbody2D.drag = value; }
        }

        public float AngularDrag
        {
            get { return rigidbody2D.angularDrag; }
            set { rigidbody2D.angularDrag = value; }
        }

        public void AddForce(Vector2 force)
        {
            rigidbody2D.AddForce(force);
        }

        public void AddTorque(float torque)
        {
            rigidbody2D.AddTorque(torque);
        }

        public void Translate(Vector2 translation)
        {
            rigidbody2D.MovePosition(rigidbody2D.position + translation);
        }
    }
}