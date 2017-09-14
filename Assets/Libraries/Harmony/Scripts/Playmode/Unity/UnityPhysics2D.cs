using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Physics2D Unity.
    /// </summary>
    /// <inheritdoc cref="IPhysics2D"/>
    [NotTested(Reason.Wrapper)]
    [AddComponentMenu("Game/Utils/UnityPhysics2D")]
    public class UnityPhysics2D : IPhysics2D
    {
        public Vector2 Gravity
        {
            get { return Physics2D.gravity; }
            set { Physics2D.gravity = value; }
        }
    }
}