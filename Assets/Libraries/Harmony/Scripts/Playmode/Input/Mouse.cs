using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Harmony
{
    [AddComponentMenu("Game/Input/Mouse")]
    public class Mouse : Script
    {
        public bool GetMouseButton(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public bool GetMouseButtonDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }
    }
}