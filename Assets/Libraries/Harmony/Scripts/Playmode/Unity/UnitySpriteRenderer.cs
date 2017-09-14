using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un SpriteRenderer Unity.
    /// </summary>
    /// <inheritdoc cref="ISpriteRenderer"/>
    [NotTested(Reason.Wrapper)]
    public class UnitySpriteRenderer : UnityRenderer, ISpriteRenderer
    {
        public UnitySpriteRenderer(SpriteRenderer spriteRenderer) : base(spriteRenderer)
        {
        }
    }
}