using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Harmony
{
    [AddComponentMenu("Game/Input/Mouse")]
    public class Mouse : Script
    {
        public bool GetMouseButton(MouseButton button)
        {
            return Input.GetMouseButton((int)button);
        }

        public bool GetMouseButtonDown(MouseButton button)
        {
            return Input.GetMouseButtonDown((int)button);
        }

        public Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }
    }
}
