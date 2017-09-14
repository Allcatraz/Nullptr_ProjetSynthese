using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un KeyboardInput Unity.
    /// </summary>
    /// <inheritdoc cref="IKeyboardInput"/>
    [NotTested(Reason.Wrapper)]
    public class UnityKeyboardInput : IKeyboardInput
    {
        public bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public bool GetKey(KeyCode key)
        {
            return Input.GetKey(key);
        }
    }
}