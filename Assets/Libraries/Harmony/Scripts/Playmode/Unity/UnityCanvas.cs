using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Canvas Unity.
    /// </summary>
    /// <inheritdoc cref="ICanvas"/>
    [NotTested(Reason.Wrapper)]
    public class UnityCanvas : UnityDisableable, ICanvas
    {
        public UnityCanvas(Canvas canvas) : base(canvas)
        {
        }
    }
}